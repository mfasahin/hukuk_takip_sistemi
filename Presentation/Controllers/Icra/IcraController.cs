using Business.Abstract;
using Entity.Dto;
using Presentation.Filters;
using System;
using System.Web.Mvc;
using FluentValidation;
using System.Linq;

namespace Presentation.Controllers
{
    [RequireLogin]
    public class IcraController : Controller
    {
        private readonly IIcraService _icraService;
        private readonly IMusteriService _musteriService;
        private readonly IAvukatService _avukatService;
        private readonly IMahkemeService _mahkemeService;
        private readonly IUrunService _urunService;

        public IcraController(
            IIcraService icraService,
            IMusteriService musteriService,
            IMahkemeService mahkemeService,
            IAvukatService avukatService,
            IUrunService urunService)
        {
            _icraService = icraService;
            _musteriService = musteriService;
            _mahkemeService = mahkemeService;
            _urunService = urunService;
            _avukatService = avukatService;
        }

        public ActionResult Index()
        {
            var model = _icraService.GetIcraDto();
                

            ViewBag.MusteriList = _icraService.GetIhtariOlanMusteriler()
                .Select(m => new SelectListItem
                {
                    Value = m.MusteriId.ToString(),
                    Text = m.MustAd + " " + m.MustSoyad
                }).ToList();

            ViewBag.AvukatList = _avukatService.GetAll()
                .Where(a => a.SIL_TAR_ZMN == null)
                .Select(a => new SelectListItem
                {
                    Value = a.AVUKAT_ID.ToString(),
                    Text = a.AVKT_AD + " " + a.AVKT_SOYAD
                }).ToList();

            ViewBag.MahkemeList = _mahkemeService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null)
                .Select(m => new SelectListItem
                {
                    Value = m.MAHKEME_ID.ToString(),
                    Text = m.MAHKEME_AD

                }).ToList();
            ViewBag.IhtarUrunList = _icraService.GetIcraDto()
                .Select(x => new SelectListItem
                {
                    Value = x.IhtarUrunId.ToString(),
                    Text = x.MusteriAd + " - " + x.UrunAd + " - " + x.IhtarTarih.ToString("dd.MM.yyyy")
                }).ToList();
            return View(model);
        }

        // 1. Kademe: Müşteri seçilince ürün listesini döndürür
        [HttpGet]
        public JsonResult GetUrunlerByMusteri(Guid musteriId, bool isForUpdate = false)
        {
            var list = _icraService.GetUrunlerByMusteri(musteriId, isForUpdate)
                .Select(x => new { value = x.UrunId, text = x.UrunAd });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // 2. Kademe: Müşteri + Ürün seçilince ihtar listesini döndürür
        [HttpGet]
        public JsonResult GetIhtarlarByMusteriVeUrun(Guid musteriId, Guid urunId)
        {
            var list = _icraService.GetIhtarlarByMusteriVeUrun(musteriId, urunId)
                .Select(x => new
                {
                    value = x.IhtarUrunId,
                    // Tarihi dd.MM.yyyy (Gün.Ay.Yıl) formatına zorluyoruz
                    text = x.IhtarTarih.ToString("dd.MM.yyyy") + " - Borç: " + x.BorcTutar.ToString("N2") + " TL"
                });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // EKLEME
        [HttpPost]
        public ActionResult Create(IcraDto model)
        {
            if (model == null)
                return Json(new { success = false, message = "Gönderilen icra verisi boş olamaz." });

            try
            {
                _icraService.Add(model);
                return Json(new { success = true, message = "İcra takibi başarıyla başlatıldı." });
            }
            catch (ValidationException ex)
            {
                var errorList = ex.Errors.Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = string.Join("<br>", errorList) });
            }
            catch (Exception ex)
            {
                // FluentValidation dışındaki tüm Exception'ların da (IcraManager vs.) mesajını doğrudan iletiyoruz
                string errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    errorMessage += "<br>" + ex.InnerException.Message;
                }

                return Json(new { success = false, message = errorMessage });
            }
        }

        // TEKİL GETİRME
        [HttpGet]
        public ActionResult GetIcra(Guid id)
        {
            var dto = _icraService.GetByIdIcra(id);
            if (dto == null) return HttpNotFound();

            return Json(new
            {
                IcraId = dto.IcraId,
                MusteriId = dto.MusteriId,
                UrunId = dto.UrunId,
                IhtarUrunId = dto.IhtarUrunId,
                MahkemeId = dto.MahkemeId,
                IcraDosyaNo = dto.IcraDosyaNo,
                IcraTakipTar = dto.IcraTakipTar.ToString("yyyy-MM-dd")
            }, JsonRequestBehavior.AllowGet);
        }

        // GÜNCELLEME
        [HttpPost]
        public ActionResult Update(IcraDto model)
        {
            if (model == null)
                return Json(new { success = false, message = "Gönderilen veri boş olamaz." });

            try
            {
                _icraService.Update(model);
                return Json(new { success = true, message = "İcra kaydı başarıyla güncellendi." });
            }
            catch (ValidationException ex)
            {
                var errorList = ex.Errors.Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = string.Join("<br>", errorList) });
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    errorMessage += "<br>" + ex.InnerException.Message;
                }

                return Json(new { success = false, message = errorMessage });
            }
        }

        // SİLME
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _icraService.Delete(id);
                return Json(new { success = true, message = "İcra kaydı başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
            }
        }
    }
}