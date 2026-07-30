using Business.Abstract;
using Entity.Concrete;
using Entity.Dto;
using FluentValidation;
using Presentation.Filters;
using Presentation.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    [RequireLogin]
    public class UrunController : Controller
    {
        private readonly IUrunService _urunService;
        private readonly IIhtarUrunService _ihtarUrunService;

        public UrunController(IUrunService urunService, IIhtarUrunService ihtarUrunService)
        {
            _urunService = urunService;
            _ihtarUrunService = ihtarUrunService;
        }

        // LİSTELEME
        public ActionResult Index()
        {
            var urunList = _urunService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null)
                .OrderByDescending(x=>x.GRS_TAR_ZMN)
                .ToList();

            var model = urunList.Select(u => new UrunModel
            {
                UrunId = u.URUN_ID,
                UrunAd = u.URUN_AD,
                UrunKod = u.URUN_KOD,
                SonGecerlilikTar = u.SON_GECERLILIK_TAR
            }).ToList();

            return View(model);
        }

        // ASENKRON ÜRÜN KODU KONTROLÜ (UI / JS Tarafından Çağrılır)
        [HttpGet]
        public ActionResult CheckUrunKodExists(string urunKod)
        {
            if (string.IsNullOrWhiteSpace(urunKod))
            {
                return Json(new { exists = false }, JsonRequestBehavior.AllowGet);
            }

            bool exists = _urunService.GetAll()
                .Any(u => u.SIL_TAR_ZMN == null && u.URUN_KOD == urunKod.Trim());

            return Json(new { exists = exists }, JsonRequestBehavior.AllowGet);
        }

        // EKLEME
        [HttpPost]
        public ActionResult Create(UrunDto model)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Gönderilen ürün verisi boş olamaz." });
            }

            try
            {
                var urun = new Urun
                {
                    URUN_ID = Guid.NewGuid(),
                    URUN_AD = model.UrunAd,
                    URUN_KOD = model.UrunKod,
                    SON_GECERLILIK_TAR = model.SonGecerlilikTar
                };

                // 1. FluentValidation Çalıştırma
                var validator = new UrunValidator();
                var result = validator.Validate(urun);

                if (!result.IsValid)
                {
                    var errorMessage = string.Join("<br>", result.Errors.Select(x => x.ErrorMessage));
                    return Json(new { success = false, message = errorMessage });
                }

                // 2. Duplicate Ürün Kodu Kontrolü
                bool isKodExists = _urunService.GetAll()
                    .Any(x => x.SIL_TAR_ZMN == null && x.URUN_KOD == urun.URUN_KOD);

                if (isKodExists)
                {
                    return Json(new { success = false, message = "Bu ürün kodu ile kayıtlı bir ürün zaten mevcut." });
                }

                // 3. Ekleme İşlemi
                _urunService.Add(urun);
                return Json(new { success = true, message = "Ürün başarıyla eklendi." });
            }
            catch (ValidationException ex)
            {
                var errorList = ex.Errors.Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = string.Join("<br>", errorList) });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Veritabanı kaydı sırasında bir hata oluştu: " + (ex.InnerException?.Message ?? ex.Message)
                });
            }
        }

        // TEKİL KAYIT (modal doldurma için)
        [HttpGet]
        public ActionResult GetUrun(Guid id)
        {
            var urun = _urunService.GetById(id);
            if (urun == null || urun.SIL_TAR_ZMN != null) return HttpNotFound();

            var model = new UrunModel
            {
                UrunId = urun.URUN_ID,
                UrunAd = urun.URUN_AD,
                UrunKod = urun.URUN_KOD,
                SonGecerlilikTar = urun.SON_GECERLILIK_TAR
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // GÜNCELLEME
        [HttpPost]
        public ActionResult Update(UrunDto model)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Gönderilen ürün verisi boş olamaz." });
            }

            try
            {
                var urun = _urunService.GetById(model.UrunId);
                if (urun == null || urun.SIL_TAR_ZMN != null)
                    return Json(new { success = false, message = "Kayıt bulunamadı." });

                urun.URUN_AD = model.UrunAd;
                urun.URUN_KOD = model.UrunKod;
                urun.SON_GECERLILIK_TAR = model.SonGecerlilikTar;

                // FluentValidation Çalıştırma
                var validator = new UrunValidator();
                var result = validator.Validate(urun);

                if (!result.IsValid)
                {
                    var errorMessage = string.Join("<br>", result.Errors.Select(x => x.ErrorMessage));
                    return Json(new { success = false, message = errorMessage });
                }

                _urunService.Update(urun);
                return Json(new { success = true, message = "Ürün başarıyla güncellendi." });
            }
            catch (ValidationException ex)
            {
                var errorList = ex.Errors.Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = string.Join("<br>", errorList) });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Güncelleme sırasında bir hata oluştu: " + (ex.InnerException?.Message ?? ex.Message)
                });
            }
        }

        // SİLME
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var urun = _urunService.GetById(id);
                if (urun == null || urun.SIL_TAR_ZMN != null)
                    return Json(new { success = false, message = "Kayıt bulunamadı." });

                // Bağımlılık kontrolü
                if (_ihtarUrunService.UruneBagliIhtarVarMi(id))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Bu ürüne bağlı ihtar kaydı bulunduğu için silinemez."
                    });
                }
                _urunService.Delete(urun);
                return Json(new { success = true, message = "Ürün başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Silme işlemi sırasında bir hata oluştu: " + (ex.InnerException?.Message ?? ex.Message)
                });
            }
        }
    }
}