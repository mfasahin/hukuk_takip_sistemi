using Business.Abstract;
using Entity.Concrete;
using Presentation.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class IhtarController : Controller
    {
        private readonly IIhtarService _ihtarService;

        // Constructor: DI ile IMusteriService enjekte ediliyor
        public IhtarController(IIhtarService ihtarService)
        {
            _ihtarService = ihtarService;
        }

        public ActionResult Index()
        {
            var ihtarList = _ihtarService.GetIhtarWithRelations();

            var model = ihtarList.Select(i => new IhtarModel
            {
                IhtarId = i.IHTAR_ID,
                BorcTutar = i.BORC_TUTAR,
                IhtarTarZmn = i.IHTAR_TAR_ZMN,
                GrsTarZmn = i.GRS_TAR_ZMN,
                GncTarZmn = i.GNC_TAR_ZMN,
                SilTarZmn = i.SIL_TAR_ZMN,

                MusteriId = i.MUSTERI_ID,
                MusteriAd = i.Musteri?.MUST_AD,   // join ile gelen navigation property
                AvukatId = i.AVUKAT_ID,
                AvukatAdSoyad = i.Avukat?.AVKT_AD,
                SubeId = i.SUBE_ID,
                SubeAd = i.Sube?.SUBE_ADI
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(IhtarModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, error = "Geçersiz model" });
            }

            try
            {
                var ihtar = new Ihtar
                {
                    IHTAR_ID = model.IhtarId,
                    BORC_TUTAR = model.BorcTutar,
                    IHTAR_TAR_ZMN = model.IhtarTarZmn,
                    GRS_TAR_ZMN = DateTime.Now
                };

                _ihtarService.Add(ihtar);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetIhtar(int id)
        {
            var ihtar = _ihtarService.GetById(id);
            if (ihtar == null) return HttpNotFound();

            var model = new IhtarModel
            {
                IhtarId = ihtar.IHTAR_ID,
                BorcTutar = ihtar.BORC_TUTAR,
                IhtarTarZmn = ihtar.IHTAR_TAR_ZMN,
                //GNC_TAR_ZMN = musteri.GNC_TAR_ZMN
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // GÜNCELLEME İŞLEMİ
        [HttpPost]
        public ActionResult Update(IhtarModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var entity = _ihtarService.GetById(model.IhtarId);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                entity.BORC_TUTAR = model.BorcTutar;
                entity.IHTAR_TAR_ZMN = model.IhtarTarZmn;
                entity.GNC_TAR_ZMN = DateTime.Now;

                _ihtarService.Update(entity);

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
                var entity = _ihtarService.GetById(id);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                // Soft delete → SilinmeTarihi doldur
                entity.SIL_TAR_ZMN = DateTime.Now;
                _ihtarService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}