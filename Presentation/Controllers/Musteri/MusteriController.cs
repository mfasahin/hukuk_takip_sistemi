using Business.Abstract;
using Entity.Concrete;
using Presentation.Models;
using System;
using System.Linq;
using System.Net;
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
            // Tüm avukatları getir
            var musteriList = _musteriService.GetAll(); // List<Entity.Concrete.Avukat>

            // SilinmeTarihi dolu olanları filtrele (yani silinmişleri gösterme)
            var aktifMusteriler = musteriList
                .Where(m => m.SIL_TAR_ZMN == null) // sadece silinmemiş kayıtlar
                .ToList();

            // Entity → ViewModel dönüşümü
            var model = aktifMusteriler.Select(m => new Presentation.Models.MusteriModel
            {
                MUSTERI_ID = m.MUSTERI_ID,
                MUST_NO = m.MUST_NO,
                MUST_AD = m.MUST_AD,
                MUST_SOYAD = m.MUST_SOYAD,
                MUST_KIMLIK_NO = m.MUST_KIMLIK_NO,
                MUST_VKNO = m.MUST_VKNO,
                MUST_EPOSTA = m.MUST_EPOSTA,
                MUST_TEL_NO = m.MUST_TEL_NO,
                SIL_TAR_ZMN = m.SIL_TAR_ZMN
            }).ToList();

            return View(model); // IEnumerable<AvukatModel> gönderiyoruz
        }

        [HttpPost]
        public ActionResult Create(Musteri musteri)
        {
            _musteriService.Add(musteri);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetMusteri(int id)
        {
            var musteri = _musteriService.GetById(id);
            if (musteri == null) return HttpNotFound();

            var model = new MusteriModel
            {
                MUSTERI_ID = musteri.MUSTERI_ID,
                MUST_NO = musteri.MUST_NO,
                MUST_AD = musteri.MUST_AD,
                MUST_SOYAD = musteri.MUST_SOYAD,
                MUST_KIMLIK_NO = musteri.MUST_KIMLIK_NO,
                MUST_VKNO = musteri.MUST_VKNO,
                MUST_EPOSTA = musteri.MUST_EPOSTA,
                MUST_TEL_NO = musteri.MUST_TEL_NO,
                //GNC_TAR_ZMN = musteri.GNC_TAR_ZMN
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        
        // GÜNCELLEME İŞLEMİ
        [HttpPost]
        public ActionResult Update(MusteriModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var entity = _musteriService.GetById(model.MUSTERI_ID);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                entity.MUST_NO = model.MUST_NO;
                entity.MUST_AD = model.MUST_AD;
                entity.MUST_SOYAD = model.MUST_SOYAD;
                entity.MUST_KIMLIK_NO= model.MUST_KIMLIK_NO;
                entity.MUST_VKNO = model.MUST_VKNO;
                entity.MUST_EPOSTA = model.MUST_EPOSTA;
                entity.MUST_TEL_NO = model.MUST_TEL_NO;
                entity.MUST_VKNO = model.MUST_VKNO;
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
        public ActionResult Delete(int id)
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