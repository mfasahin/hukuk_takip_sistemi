using Business.Abstract;
using Entity.Concrete;
using Entity.Dto;
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
        private readonly IIhtarUrunService _ihtarUrunService;

        public IhtarController(
            IIhtarService ihtarService,
            IMusteriService musteriService,
            ISubeService subeService,
            IAvukatService avukatService,
            IIhtarUrunService ihtarUrunService,
            IUrunService urunService)
        {
            _ihtarService = ihtarService;
            _musteriService = musteriService;
            _subeService = subeService;
            _avukatService = avukatService;
            _ihtarUrunService = ihtarUrunService;
            _urunService = urunService;
        }

        public ActionResult Index()
        {
            var ihtarListesi = _ihtarService.GetIhtarWithRelations();

            ViewBag.MusteriList = _musteriService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null)
                .Select(m => new SelectListItem
                {
                    Value = m.MUSTERI_ID.ToString(),
                    Text = m.MUST_AD + " " + m.MUST_SOYAD
                }).ToList();

            ViewBag.SubeList = _subeService.GetAll()
                .Where(s => s.SIL_TAR_ZMN == null)
                .Select(s => new SelectListItem
                {
                    Value = s.SUBE_ID.ToString(),
                    Text = s.SUBE_ADI
                }).ToList();

            ViewBag.AvukatList = _avukatService.GetAll()
                .Where(a => a.SIL_TAR_ZMN == null)
                .Select(a => new SelectListItem
                {
                    Value = a.AVUKAT_ID.ToString(),
                    Text = a.AVKT_AD
                }).ToList();

            // Çoklu seçim (multi-select) dropdown için
            ViewBag.UrunList = _urunService.GetAll()
                .Where(u => u.SIL_TAR_ZMN == null)
                .Select(u => new SelectListItem
                {
                    Value = u.URUN_ID.ToString(),
                    Text = u.URUN_AD
                }).ToList();

            return View(ihtarListesi);
        }

        [HttpGet]
        public ActionResult GetIhtar(Guid id)
        {
            var ihtarDto = _ihtarService.GetByIdWithRelations(id);
            if (ihtarDto == null) return HttpNotFound();

            return Json(ihtarDto, JsonRequestBehavior.AllowGet);
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
                var ihtar = new Ihtar
                {
                    IHTAR_ID = Guid.NewGuid(),
                    BORC_TUTAR = model.BorcTutar,
                    IHTAR_TAR_ZMN = model.IhtarTarih,
                    MUSTERI_ID = model.MusteriId,
                    AVUKAT_ID = model.AvukatId,
                    SUBE_ID = model.SubeId,
                    GRS_TAR_ZMN = DateTime.Now
                };

                ihtar.IhtarUrunler = new List<IhtarUrun>();
                if (model.SecilenUrunler != null)
                {
                    foreach (var urunId in model.SecilenUrunler.Distinct())
                    {
                        ihtar.IhtarUrunler.Add(new IhtarUrun
                        {
                            IHTAR_URUN_ID = Guid.NewGuid(),
                            IHTAR_ID = ihtar.IHTAR_ID,
                            URUN_ID = urunId,
                            GRS_TAR_ZMN = DateTime.Now
                        });
                    }
                }

                _ihtarService.Add(ihtar);

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
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var ihtar = _ihtarService.GetEntityWithUrunlerIncluded(model.IhtarId);
                if (ihtar == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                ihtar.BORC_TUTAR = model.BorcTutar;
                ihtar.IHTAR_TAR_ZMN = model.IhtarTarih;
                ihtar.MUSTERI_ID = model.MusteriId;
                ihtar.AVUKAT_ID = model.AvukatId;
                ihtar.SUBE_ID = model.SubeId;
                ihtar.GNC_TAR_ZMN = DateTime.Now;
                
                _ihtarService.Update(ihtar);

                foreach (var eu in ihtar.IhtarUrunler.ToList())
                    _ihtarUrunService.Delete(eu);

                if (model.Urunler != null)
                {
                    foreach (var urunDto in model.Urunler)
                    {
                        var yeni = new IhtarUrun
                        {
                            IHTAR_ID = ihtar.IHTAR_ID,
                            URUN_ID = urunDto.UrunId
                        };
                        _ihtarUrunService.Add(yeni);
                    }
                }

                return Json(new { success = true });


            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // SİLME (soft delete)
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var ihtar = _ihtarService.GetById(id);
                if (ihtar == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                ihtar.SIL_TAR_ZMN = DateTime.Now;
                _ihtarService.Update(ihtar);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}