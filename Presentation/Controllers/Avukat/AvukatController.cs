using Business.Abstract;
using Business.Validation;
using Entity.Concrete;
using FluentValidation;
using Presentation.Filters;
using Presentation.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    [RequireLogin]
    public class AvukatController : Controller
    {
        private readonly IAvukatService _avukatService;
        private readonly IIhtarService _ihtarService;

        public AvukatController(IAvukatService avukatService, IIhtarService ihtarService)
        {
            _avukatService = avukatService;
            _ihtarService = ihtarService;
        }

        // LİSTELEME
        public ActionResult Index()
        {
            var avukatList = _avukatService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null);

            var model = avukatList.Select(m => new AvukatModel
            {
                AvukatId = m.AVUKAT_ID,
                AvktAd = m.AVKT_AD,
                AvktSoyad = m.AVKT_SOYAD,
                TbbSicilNo = m.TBB_SICIL_NO,
                AvktTelNo = m.AVKT_TEL_NO,
                AvktEposta = m.AVKT_EPOSTA,
                HkkBuroAd = m.HKK_BURO_AD,
                HkkBuroAdres = m.HKK_BURO_ADRES,
                OfisTelNo = m.OFIS_TEL_NO
            }).ToList();

            return View(model);
        }

        // ASENKRON TBB SİCİL NO KONTROLÜ (UI / JS Tarafından Çağrılır)
        [HttpGet]
        public ActionResult CheckTbbSicilNoExists(string tbbSicilNo)
        {
            if (string.IsNullOrWhiteSpace(tbbSicilNo))
            {
                return Json(new { exists = false }, JsonRequestBehavior.AllowGet);
            }

            bool exists = _avukatService.GetAll()
                .Any(a => a.SIL_TAR_ZMN == null && a.TBB_SICIL_NO == tbbSicilNo.Trim());

            return Json(new { exists = exists }, JsonRequestBehavior.AllowGet);
        }

        // EKLEME
        [HttpPost]
        public ActionResult Create(AvukatModel model)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Gönderilen avukat verisi boş olamaz." });
            }

            try
            {
                var avukat = new Avukat
                {
                    AVUKAT_ID = Guid.NewGuid(),
                    AVKT_AD = model.AvktAd,
                    AVKT_SOYAD = model.AvktSoyad,
                    TBB_SICIL_NO = model.TbbSicilNo,
                    AVKT_TEL_NO = model.AvktTelNo,
                    AVKT_EPOSTA = model.AvktEposta,
                    HKK_BURO_AD = model.HkkBuroAd,
                    HKK_BURO_ADRES = model.HkkBuroAdres,
                    OFIS_TEL_NO = model.OfisTelNo
                };

                // 1. FluentValidation Çalıştırma
                var validator = new AvukatValidator();
                var result = validator.Validate(avukat);

                if (!result.IsValid)
                {
                    var errorMessage = string.Join("<br>", result.Errors.Select(x => x.ErrorMessage));
                    return Json(new { success = false, message = errorMessage });
                }

                // 2. Duplicate TBB Sicil No Kontrolü
                bool isSicilExists = _avukatService.GetAll()
                    .Any(a => a.SIL_TAR_ZMN == null && a.TBB_SICIL_NO == avukat.TBB_SICIL_NO);

                if (isSicilExists)
                {
                    return Json(new { success = false, message = "Bu TBB Sicil Numarası ile kayıtlı bir avukat zaten mevcut." });
                }

                // 3. Ekleme İşlemi
                _avukatService.Add(avukat);
                return Json(new { success = true, message = "Avukat başarıyla eklendi." });
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
        public ActionResult GetAvukat(Guid id)
        {
            var avukat = _avukatService.GetById(id);
            if (avukat == null || avukat.SIL_TAR_ZMN != null) return HttpNotFound();

            var model = new AvukatModel
            {
                AvukatId = avukat.AVUKAT_ID,
                AvktAd = avukat.AVKT_AD,
                AvktSoyad = avukat.AVKT_SOYAD,
                TbbSicilNo = avukat.TBB_SICIL_NO,
                AvktTelNo = avukat.AVKT_TEL_NO,
                AvktEposta = avukat.AVKT_EPOSTA,
                HkkBuroAd = avukat.HKK_BURO_AD,
                HkkBuroAdres = avukat.HKK_BURO_ADRES,
                OfisTelNo = avukat.OFIS_TEL_NO
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // GÜNCELLEME
        [HttpPost]
        public ActionResult Update(AvukatModel model)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Gönderilen avukat verisi boş olamaz." });
            }

            try
            {
                var avukat = _avukatService.GetById(model.AvukatId);
                if (avukat == null || avukat.SIL_TAR_ZMN != null)
                    return Json(new { success = false, message = "Kayıt bulunamadı." });

                avukat.AVKT_AD = model.AvktAd;
                avukat.AVKT_SOYAD = model.AvktSoyad;
                avukat.TBB_SICIL_NO = model.TbbSicilNo;
                avukat.AVKT_TEL_NO = model.AvktTelNo;
                avukat.AVKT_EPOSTA = model.AvktEposta;
                avukat.HKK_BURO_AD = model.HkkBuroAd;
                avukat.HKK_BURO_ADRES = model.HkkBuroAdres;
                avukat.OFIS_TEL_NO = model.OfisTelNo;

                // FluentValidation Çalıştırma
                var validator = new AvukatValidator();
                var result = validator.Validate(avukat);

                if (!result.IsValid)
                {
                    var errorMessage = string.Join("<br>", result.Errors.Select(x => x.ErrorMessage));
                    return Json(new { success = false, message = errorMessage });
                }

                _avukatService.Update(avukat);
                return Json(new { success = true, message = "Avukat başarıyla güncellendi." });
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
                var avukat = _avukatService.GetById(id);
                if (avukat == null || avukat.SIL_TAR_ZMN != null)
                    return Json(new { success = false, message = "Kayıt bulunamadı." });

                // Bağımlılık kontrolü
                if (_ihtarService.AvukataBagliIhtarVarMi(id))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Bu avukata bağlı ihtar kaydı bulunduğu için silinemez."
                    });
                }

                // Soft Delete uygula
                avukat.SIL_TAR_ZMN = DateTime.Now;
                _avukatService.Update(avukat);

                return Json(new { success = true, message = "Avukat başarıyla silindi." });
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