using Business.Abstract;
using Entity.Concrete;
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

        public UrunController(IUrunService urunService)
        {
            _urunService = urunService;
        }

        // LİSTELEME
        public ActionResult Index()
        {
            var urunList = _urunService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null);

            var model = urunList.Select(u => new UrunModel
            {
                UrunId = u.URUN_ID,
                UrunAd = u.URUN_AD,
                UrunKod = u.URUN_KOD,
                SonGecerlilikTar = u.SON_GECERLILIK_TAR

            }).ToList();

            return View(model);
        }

        // EKLEME
        [HttpPost]
        public ActionResult Create(UrunModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var urun = new Urun
                {
                    URUN_ID = Guid.NewGuid(),
                    URUN_AD = model.UrunAd,
                    URUN_KOD = model.UrunKod,
                    SON_GECERLILIK_TAR = model.SonGecerlilikTar,
                };

                _urunService.Add(urun);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // TEKİL KAYIT (modal doldurma için)
        [HttpGet]
        public ActionResult GetUrun(Guid id)
        {
            var urun = _urunService.GetById(id);
            if (urun == null) return HttpNotFound();

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
        public ActionResult Update(UrunModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var urun = _urunService.GetById(model.UrunId);
                if (urun == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                urun.URUN_AD = model.UrunAd;
                urun.URUN_KOD = model.UrunKod;
                urun.SON_GECERLILIK_TAR = model.SonGecerlilikTar;

                _urunService.Update(urun);
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
                var urun = _urunService.GetById(id);
                if (urun == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                _urunService.Delete(urun);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}