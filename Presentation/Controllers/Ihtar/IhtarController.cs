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
        private readonly IMusteriService _musteriService;
        private readonly IAvukatService _avukatService;
        private readonly ISubeService _subeService;


        // Constructor: DI ile IMusteriService enjekte ediliyor
        public IhtarController(
            IIhtarService ihtarService,
            IMusteriService musteriService,
            IAvukatService avukatService,
            ISubeService subeService)
        {
            _ihtarService = ihtarService;
            _musteriService = musteriService;
            _avukatService = avukatService;
            _subeService = subeService;
        }

        public ActionResult Index()
        {
            var ihtarList = _ihtarService.GetIhtarWithRelations();

            var model = ihtarList.Select(i => new IhtarModel
            {
                IhtarId = i.IHTAR_ID,
                BorcTutar = i.BORC_TUTAR,
                IhtarTarZmn = i.IHTAR_TAR_ZMN,
                MusteriId = i.MUSTERI_ID,
                AvukatId = i.AVUKAT_ID,
                SubeId = i.SUBE_ID
            }).ToList();

            // Dropdown listeleri doldur
            ViewBag.MusteriList = _musteriService.GetAll()
                .Select(m => new SelectListItem
                {
                    Value = m.MUSTERI_ID.ToString(),
                    Text = m.MUST_AD
                }).ToList();

            ViewBag.AvukatList = _avukatService.GetAll()
                .Select(a => new SelectListItem
                {
                    Value = a.AVUKAT_ID.ToString(),
                    Text = a.AVKT_AD
                }).ToList();

            ViewBag.SubeList = _subeService.GetAll()
                .Select(s => new SelectListItem
                {
                    Value = s.SUBE_ID.ToString(),
                    Text = s.SUBE_ADI
                }).ToList();

            return View(model);
        }


        [HttpPost]
        public ActionResult Create(IhtarModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, error = "Geçersiz model", details = errors });
            }

            try
            {
                var ihtar = new Ihtar
                {
                    BORC_TUTAR = model.BorcTutar,
                    IHTAR_TAR_ZMN = model.IhtarTarZmn,
                    MUSTERI_ID = model.MusteriId,
                    SUBE_ID = model.SubeId,
                    AVUKAT_ID = model.AvukatId,
                    GRS_TAR_ZMN = DateTime.Now
                };

                _ihtarService.Add(ihtar);

                // İsimleri doldur
                var musteriAd = _musteriService.GetById(model.MusteriId)?.MUST_AD;
                var subeAd = _subeService.GetById(model.SubeId)?.SUBE_ADI;
                var avukatAdSoyad = _avukatService.GetById(model.AvukatId)?.AVKT_AD;

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        ihtarId = ihtar.IHTAR_ID,
                        borcTutar = ihtar.BORC_TUTAR,
                        ihtarTarZmn = ihtar.IHTAR_TAR_ZMN.ToString("dd.MM.yyyy"),
                        musteriAd = musteriAd,
                        subeAd = subeAd,
                        avukatAdSoyad = avukatAdSoyad
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Sunucu hatası", details = ex.Message });
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
        //public ActionResult Edit(int id)
        //{
        //    var entity = _ihtarService.GetById(id);
        //    if (entity == null)
        //        return HttpNotFound();

        //    var model = new IhtarModel
        //    {
        //        IhtarId = entity.IHTAR_ID,
        //        BorcTutar = entity.BORC_TUTAR,
        //        IhtarTarZmn = entity.IHTAR_TAR_ZMN,
        //        MusteriId = entity.MUSTERI_ID,
        //        SubeId = entity.SUBE_ID,
        //        AvukatId = entity.AVUKAT_ID
        //    };

        //    ViewBag.MusteriList = new SelectList(_musteriService.GetAll(), "MUSTERI_ID", "MUSTERI_AD", model.MusteriId);
        //    ViewBag.SubeList = new SelectList(_subeService.GetAll(), "SUBE_ID", "SUBE_AD", model.SubeId);
        //    ViewBag.AvukatList = new SelectList(_avukatService.GetAll(), "AVUKAT_ID", "AVUKAT_ADSOYAD", model.AvukatId);

        //    return PartialView("_IhtarModals", model);
        //}

        [HttpGet]
        public ActionResult Update(int id)
        {
            var entity = _ihtarService.GetById(id);
            if (entity == null)
                return HttpNotFound();

            var model = new IhtarModel
            {
                IhtarId = entity.IHTAR_ID,
                BorcTutar = entity.BORC_TUTAR,
                IhtarTarZmn = entity.IHTAR_TAR_ZMN,
                MusteriId = entity.MUSTERI_ID,   // <-- bu satırlar eksikse
                AvukatId = entity.AVUKAT_ID,    // <-- dropdown boş kalır
                SubeId = entity.SUBE_ID
            };

            // Create action'ındaki ile AYNI şekilde dolduruluyor olmalı
            ViewBag.MusteriList = _musteriService.GetAll()
                .Select(m => new SelectListItem { Value = m.MUSTERI_ID.ToString(), Text = m.MUST_AD + " " + m.MUST_SOYAD })
                .ToList();

            ViewBag.SubeList = _subeService.GetAll()
                .Select(s => new SelectListItem { Value = s.SUBE_ID.ToString(), Text = s.SUBE_ADI })
                .ToList();

            ViewBag.AvukatList = _avukatService.GetAll()
                .Select(a => new SelectListItem { Value = a.AVUKAT_ID.ToString(), Text = a.AVKT_AD + " " + a.AVKT_SOYAD })
                .ToList();

            return PartialView("_UpdatePartial", model);
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