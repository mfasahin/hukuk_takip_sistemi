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
                UrunId = u.URUN_ID,
                UrunAd = u.URUN_AD,
                UrunKod = u.URUN_KOD,
                SonGecerlilikTar = u.SON_GECERLILIK_TAR,
                SilTarZmn = u.SIL_TAR_ZMN
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
                    URUN_ID = model.UrunId,
                    URUN_AD = model.UrunAd,
                    URUN_KOD = model.UrunKod,
                    SON_GECERLILIK_TAR = model.SonGecerlilikTar,
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
                UrunId = urun.URUN_ID,
                UrunAd = urun.URUN_AD,
                UrunKod = urun.URUN_KOD,
                SonGecerlilikTar = urun.SON_GECERLILIK_TAR,
                //GNC_TAR_ZMN = musteri.GNC_TAR_ZMN
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // GÜNCELLEME 
        [HttpPost]
        public ActionResult Update(UrunModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var entity = _urunService.GetById(model.UrunId);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                entity.URUN_AD = model.UrunAd;
                entity.URUN_KOD = model.UrunKod;
                entity.SON_GECERLILIK_TAR = model.SonGecerlilikTar;
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