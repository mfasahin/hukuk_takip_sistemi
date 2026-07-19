using Business.Abstract;
using Entity.Concrete;
using Presentation.Models;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class MusteriController : Controller
    {
        private readonly IMusteriService _musteriService;

        // Constructor: DI ile IMusteriService enjekte ediliyor
        public MusteriController(IMusteriService musteriService)
        {
            _musteriService = musteriService;
        }

        public ActionResult Index()
        {
            // Tüm müsterileri getir
            var musteriList = _musteriService.GetAll().Where(m => m.SIL_TAR_ZMN == null) // sadece silinmemiş kayıtlar
                .ToList(); // List<Entity.Concrete.Musteri>


            // Entity → ViewModel dönüşümü
            var model = musteriList.Select(m => new Presentation.Models.MusteriModel
            {
                MusteriId = m.MUSTERI_ID,
                MustNo = m.MUST_NO,
                MustAd = m.MUST_AD,
                MustSoyad = m.MUST_SOYAD,
                MustKimlikNo = m.MUST_KIMLIK_NO,
                MustVknNo = m.MUST_VKN_NO,
                MustEposta = m.MUST_EPOSTA,
                MustTelNo = m.MUST_TEL_NO,
                SilTarZmn = m.SIL_TAR_ZMN
            }).ToList();

            return View(model); // IEnumerable<AvukatModel> gönderiyoruz
        }

        [HttpPost]
        public ActionResult Create(MusteriModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                return Json(new { success = false, error = string.Join("; ", errors) });
            }

            try
            {
                var musteri = new Musteri
                {
                    MUST_NO = model.MustNo,
                    MUST_AD = model.MustAd,
                    MUST_SOYAD = model.MustSoyad,
                    MUST_KIMLIK_NO = model.MustKimlikNo,
                    MUST_VKN_NO = model.MustVknNo,
                    MUST_EPOSTA = model.MustEposta,
                    MUST_TEL_NO = model.MustTelNo,
                    GRS_TAR_ZMN = DateTime.Now
                };

                _musteriService.Add(musteri);

                return Json(new { success = true });
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => $"Property: {e.PropertyName}, Error: {e.ErrorMessage}");
                return Json(new { success = false, error = string.Join("; ", errors) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


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
                MustTelNo = musteri.MUST_TEL_NO,
                //GNC_TAR_ZMN = musteri.GNC_TAR_ZMN
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
                var entity = _musteriService.GetById(model.MusteriId);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                entity.MUST_NO = model.MustNo;
                entity.MUST_AD = model.MustAd;
                entity.MUST_SOYAD = model.MustSoyad;
                entity.MUST_KIMLIK_NO = model.MustKimlikNo;
                entity.MUST_VKN_NO = model.MustVknNo;
                entity.MUST_EPOSTA = model.MustEposta;
                entity.MUST_TEL_NO = model.MustTelNo;
                entity.GNC_TAR_ZMN = DateTime.Now;

                _musteriService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        //SİLME
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var entity = _musteriService.GetById(id);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                // Soft delete → SilinmeTarihi doldur
                entity.SIL_TAR_ZMN = DateTime.Now;
                _musteriService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}