using Business.Abstract;
using Business.Validation;
using Entity.Concrete;
using FluentValidation;
using Presentation.Filters;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Presentation.Controllers
{
    [RequireLogin]
    public class MusteriController : Controller
    {
        private readonly IMusteriService _musteriService;
        private readonly IIhtarService _ihtarService;

        public MusteriController(IMusteriService musteriService, IIhtarService ihtarService)
        {
            _musteriService = musteriService;
            _ihtarService = ihtarService;
        }

        // LİSTELEME
        public ActionResult Index()
        {
            var musteriList = _musteriService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null);

            var model = musteriList.Select(m => new MusteriModel
            {
                MusteriId = m.MUSTERI_ID,
                MustNo = m.MUST_NO,
                MustAd = m.MUST_AD,
                MustSoyad = m.MUST_SOYAD,
                MustKimlikNo = m.MUST_KIMLIK_NO,
                MustVknNo = m.MUST_VKN_NO,
                MustEposta = m.MUST_EPOSTA,
                MustTelNo = m.MUST_TEL_NO
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MusteriModel model)
        {
            // Model tamamen null geldiyse hemen hata dön
            if (model == null)
            {
                return Json(new { success = false, message = "Gönderilen müşteri verisi boş olamaz." });
            }

            try
            {
                // 1. Müşteri Modelinden Entity Dönüşümü
                var musteri = new Musteri
                {
                    MUSTERI_ID = Guid.NewGuid(),
                    MUST_NO = model.MustNo,
                    MUST_AD = model.MustAd,
                    MUST_SOYAD = model.MustSoyad,
                    MUST_KIMLIK_NO = model.MustKimlikNo,
                    MUST_VKN_NO = model.MustVknNo,
                    MUST_EPOSTA = model.MustEposta,
                    MUST_TEL_NO = model.MustTelNo
                };

                // 2. Fluent Validation Çalıştırma
                var validator = new MusteriValidator();
                var result = validator.Validate(musteri);

                if (!result.IsValid)
                {
                    // Hataları ön yüzde gösterilmek üzere HTML formatında birleştiriyoruz
                    var errorList = result.Errors.Select(x => x.ErrorMessage).ToList();
                    string errorMessage = string.Join("<br>", errorList);

                    return Json(new { success = false, message = errorMessage });
                }

                // 3. Servis Katmanı Üzerinden Veritabanına Ekleme
                _musteriService.Add(musteri);

                return Json(new { success = true, message = "Müşteri başarıyla eklendi." });
            }
            catch (ValidationException ex)
            {
                // FluentValidation Exception fırlatırsa yakala
                var errorList = ex.Errors.Select(e => e.ErrorMessage).ToList();
                string errorMessage = string.Join("<br>", errorList);

                return Json(new { success = false, message = errorMessage });
            }
            catch (Exception ex)
            {
                // Veritabanı veya sunucu taraflı genel hataları yakala
                return Json(new
                {
                    success = false,
                    message = "Veritabanına kayıtsı sırasında bir hata oluştu: " + (ex.InnerException?.Message ?? ex.Message)
                });
            }
        }

        // TEKİL KAYIT (modal doldurma için)
        [HttpGet]
        public ActionResult GetMusteri(Guid id)
        {
            var musteri = _musteriService.GetById(id);
            if (musteri == null) return HttpNotFound();

            var model = new MusteriModel
            {
                MusteriId = musteri.MUSTERI_ID,
                MustNo = musteri.MUST_NO,
                MustAd = musteri.MUST_AD,
                MustSoyad = musteri.MUST_SOYAD,
                MustKimlikNo = musteri.MUST_KIMLIK_NO,
                MustVknNo = musteri.MUST_VKN_NO,
                MustEposta = musteri.MUST_EPOSTA,
                MustTelNo = musteri.MUST_TEL_NO
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // GÜNCELLEME
        [HttpPost]
        public ActionResult Update(MusteriModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var musteri = _musteriService.GetById(model.MusteriId);
                if (musteri == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                musteri.MUST_NO = model.MustNo;
                musteri.MUST_AD = model.MustAd;
                musteri.MUST_SOYAD = model.MustSoyad;
                musteri.MUST_KIMLIK_NO = model.MustKimlikNo;
                musteri.MUST_VKN_NO = model.MustVknNo;
                musteri.MUST_EPOSTA = model.MustEposta;
                musteri.MUST_TEL_NO = model.MustTelNo;

                _musteriService.Update(musteri);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // SİLME
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var musteri = _musteriService.GetById(id);
                if (musteri == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                if (_ihtarService.MusteriyeBagliIhtarVarMi(id))
                {
                    return Json(new
                    {
                        success = false,
                        error = "Bu müşteriye bağlı ihtar kaydı bulunduğu için silinemez."
                    });
                }

                _musteriService.Delete(musteri);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}