using Business.Abstract;
using Entity.Concrete;
using Presentation.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class UrunController : Controller
    {
        private readonly IUrunService _urunService;

        // Constructor: DI ile IMusteriService enjekte ediliyor
        public UrunController(IUrunService urunService)
        {
            _urunService = urunService;
        }

        public ActionResult Index()
        {
            // Tüm ürünleri getir
            var urunList = _urunService.GetAll(); // List<Entity.Concrete.Urun>

            // SilinmeTarihi dolu olanları filtrele (yani silinmişleri gösterme)
            var aktifUrunler = urunList
                .Where(u => u.SIL_TAR_ZMN == null) // sadece silinmemiş kayıtlar
                .ToList();

            // Entity → ViewModel dönüşümü
            var model = aktifUrunler.Select(u => new Presentation.Models.UrunModel
            {
                URUN_ID = u.URUN_ID,
                URUN_AD = u.URUN_AD,
                URUN_KOD = u.URUN_KOD,
                SON_GECERLILIK_TAR = u.SON_GECERLILIK_TAR,
                SIL_TAR_ZMN = u.SIL_TAR_ZMN
            }).ToList();

            return View(model); // IEnumerable<UrunModel> gönderiyoruz
        }

        [HttpPost]
        public ActionResult Create(UrunModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, error = "Geçersiz model" });
            }

            try
            {
                var urun = new Urun
                {
                    URUN_ID = model.URUN_ID,
                    URUN_AD = model.URUN_AD,
                    URUN_KOD = model.URUN_KOD,
                    SON_GECERLILIK_TAR = model.SON_GECERLILIK_TAR,
                    GRS_TAR_ZMN = DateTime.Now
                };

                _urunService.Add(urun);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


        [HttpGet]
        public ActionResult GetUrun(int id)
        {
            var urun = _urunService.GetById(id);
            if (urun == null) return HttpNotFound();

            var model = new UrunModel
            {
                URUN_ID = urun.URUN_ID,
                URUN_AD = urun.URUN_AD,
                URUN_KOD = urun.URUN_KOD,
                SON_GECERLILIK_TAR = urun.SON_GECERLILIK_TAR,
                //GNC_TAR_ZMN = musteri.GNC_TAR_ZMN
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // GÜNCELLEME İŞLEMİ
        [HttpPost]
        public ActionResult Update(UrunModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var entity = _urunService.GetById(model.URUN_ID);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                entity.URUN_AD = model.URUN_AD;
                entity.URUN_KOD = model.URUN_KOD;
                entity.SON_GECERLILIK_TAR = model.SON_GECERLILIK_TAR;
                entity.GNC_TAR_ZMN = DateTime.Now;

                _urunService.Update(entity);

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
                var entity = _urunService.GetById(id);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                // Soft delete → SilinmeTarihi doldur
                entity.SIL_TAR_ZMN = DateTime.Now;
                _urunService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}