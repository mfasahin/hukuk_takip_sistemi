using Business.Abstract;
using Entity.Concrete;
using Entity.Dto;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class IhtarController : Controller
    {
        private readonly IIhtarService _ihtarService;
        private readonly IMusteriService _musteriService;
        private readonly ISubeService _subeService;
        private readonly IAvukatService _avukatService;
        private readonly IUrunService _urunService;

        public IhtarController(
            IIhtarService ihtarService,
            IMusteriService musteriService,
            ISubeService subeService,
            IAvukatService avukatService,
            IUrunService urunService)
        {
            _ihtarService = ihtarService;
            _musteriService = musteriService;
            _subeService = subeService;
            _avukatService = avukatService;
            _urunService = urunService;
        }

        public ActionResult Index()
        {
            var ihtarList = _ihtarService.GetIhtarWithRelations();

            var model = ihtarList.Select(i => new IhtarDto
            {
                IhtarId = i.IhtarId,
                BorcTutar = i.BorcTutar,
                IhtarTarih = i.IhtarTarih,
                MusteriId = i.MusteriId,
                MusteriAd = i.MusteriAd,
                AvukatId = i.AvukatId,
                AvukatAd = i.AvukatAd,
                SubeId = i.SubeId,
                SubeAd = i.SubeAd,
                UrunId = i.UrunId,
                UrunAd = i.UrunAd,
                SilTarZmn = i.SilTarZmn
            }).ToList();

            ViewBag.MusteriList = _musteriService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null)
                .Select(m => new SelectListItem
                {
                    Value = m.MUSTERI_ID.ToString(),
                    Text = m.MUST_AD + " " + m.MUST_SOYAD
                }).ToList();

            ViewBag.SubeList = _subeService.GetAll()
                .Select(s => new SelectListItem
                {
                    Value = s.SUBE_ID.ToString(),
                    Text = s.SUBE_ADI
                }).ToList();

            ViewBag.AvukatList = _avukatService.GetAll()
                .Select(a => new SelectListItem
                {
                    Value = a.AVUKAT_ID.ToString(),
                    Text = a.AVKT_AD
                }).ToList();

            ViewBag.UrunList = _urunService.GetAll()
                .Select(u => new SelectListItem
                {
                    Value = u.URUN_ID.ToString(),
                    Text = u.URUN_AD
                }).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult GetIhtar(int id)
        {
            var ihtar = _ihtarService.GetByIdWithRelations(id);
            if (ihtar == null) return HttpNotFound();

            var urun = ihtar.IhtarUrunler?.FirstOrDefault();

            var dto = new IhtarDto
            {
                IhtarId = ihtar.IHTAR_ID,
                BorcTutar = ihtar.BORC_TUTAR,
                IhtarTarih = ihtar.IHTAR_TAR_ZMN,
                MusteriId = ihtar.MUSTERI_ID,
                MusteriAd = ihtar.Musteri?.MUST_AD ?? string.Empty,
                AvukatId = ihtar.AVUKAT_ID,
                AvukatAd = ihtar.Avukat?.AVKT_AD ?? string.Empty,
                SubeId = ihtar.SUBE_ID,
                SubeAd = ihtar.Sube?.SUBE_ADI ?? string.Empty,
                UrunId = urun?.URUN_ID ?? 0,
                UrunAd = urun?.Urun?.URUN_AD ?? string.Empty,
                SilTarZmn = ihtar.SIL_TAR_ZMN
            };

            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        // EKLEME
        [HttpPost]
        public ActionResult Create(IhtarDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, error = "ModelState geçersiz", details = errors });
            }

            try
            {
                var entity = new Ihtar
                {
                    BORC_TUTAR = model.BorcTutar,
                    IHTAR_TAR_ZMN = model.IhtarTarih,
                    MUSTERI_ID = model.MusteriId,
                    AVUKAT_ID = model.AvukatId,
                    SUBE_ID = model.SubeId,
                    GRS_TAR_ZMN = DateTime.Now
                };

                if (model.UrunId > 0)
                {
                    entity.IhtarUrunler = new List<IhtarUrun>
                    {
                        new IhtarUrun { URUN_ID = model.UrunId }
                    };
                }

                _ihtarService.Add(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // GÜNCELLEME
        [HttpPost]
        public ActionResult Update(IhtarDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    })
                    .ToList();

                return Json(new { success = false, error = "ModelState geçersiz", details = errors });
            }
            try
            {
                var entity = _ihtarService.GetByIdWithRelations(model.IhtarId);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                entity.BORC_TUTAR = model.BorcTutar;
                entity.IHTAR_TAR_ZMN = model.IhtarTarih;
                entity.MUSTERI_ID = model.MusteriId;
                entity.AVUKAT_ID = model.AvukatId;
                entity.SUBE_ID = model.SubeId;

                if (model.UrunId > 0)
                {
                    if (entity.IhtarUrunler == null)
                        entity.IhtarUrunler = new List<IhtarUrun>();

                    var urunRelation = entity.IhtarUrunler.FirstOrDefault();
                    if (urunRelation != null)
                    {
                        urunRelation.URUN_ID = model.UrunId;
                    }
                    else
                    {
                        entity.IhtarUrunler.Add(new IhtarUrun
                        {
                            URUN_ID = model.UrunId,
                            IHTAR_ID = entity.IHTAR_ID,
                            GRS_TAR_ZMN = DateTime.Now   // <-- eklendi: tarih alanı boş kalmasın diye
                        });
                    }
                }

                entity.GNC_TAR_ZMN = DateTime.Now;

                _ihtarService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var inner = ex.InnerException;
                while (inner != null)
                {
                    message += " | INNER: " + inner.Message;
                    inner = inner.InnerException;
                }

                return Json(new { success = false, error = message });
            }
        }

        // SİLME (soft delete)
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var entity = _ihtarService.GetById(id);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

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