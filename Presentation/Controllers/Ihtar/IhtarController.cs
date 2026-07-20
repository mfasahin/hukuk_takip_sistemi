using Business.Abstract;
using Presentation.Mapping;
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
            var viewModel = new IhtarIndexViewModel
            {
                Ihtarlar = _ihtarService.GetIhtarWithRelations()
                    .Select(e => e.ToModel())
                    .ToList(),

                MusteriList = _musteriService.GetAll()
                    .Where(m => m.SIL_TAR_ZMN == null)
                    .Select(m => new SelectListItem
                    {
                        Value = m.MUSTERI_ID.ToString(),
                        Text = m.MUST_AD + " " + m.MUST_SOYAD
                    }).ToList(),

                SubeList = _subeService.GetAll()
                    .Select(s => new SelectListItem
                    {
                        Value = s.SUBE_ID.ToString(),
                        Text = s.SUBE_ADI
                    }).ToList(),

                AvukatList = _avukatService.GetAll()
                    .Select(a => new SelectListItem
                    {
                        Value = a.AVUKAT_ID.ToString(),
                        Text = a.AVKT_AD
                    }).ToList(),

                UrunList = _urunService.GetAll()
                    .Select(u => new SelectListItem
                    {
                        Value = u.URUN_ID.ToString(),
                        Text = u.URUN_AD
                    }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult GetIhtar(Guid id)
        {
            var entity = _ihtarService.GetByIdWithRelations(id);
            if (entity == null) return HttpNotFound();

            return Json(entity.ToModel(), JsonRequestBehavior.AllowGet);
        }

        // EKLEME
        [HttpPost]
        public ActionResult Create(IhtarModel model)
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
                var entity = model.ToEntity();
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
        public ActionResult Update(IhtarModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var entity = _ihtarService.GetByIdWithRelations(model.IhtarId);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                model.ApplyTo(entity);
                model.SyncUrunler(entity);
                entity.GNC_TAR_ZMN = DateTime.Now;

                _ihtarService.Update(entity);

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