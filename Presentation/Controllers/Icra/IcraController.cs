using Business.Abstract;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class IcraController : Controller
    {
        private readonly IIcraService _icraService;
        private readonly IMahkemeService _mahkemeService;
        private readonly IIhtarUrunService _ihtarUrunService;
        private readonly IUrunService _urunService;

        public IcraController(
            IIcraService icraService,
            IMahkemeService mahkemeService,
            IIhtarUrunService ihtarUrunService,
            IUrunService urunService)
        {
            _icraService = icraService;
            _mahkemeService = mahkemeService;
            _ihtarUrunService = ihtarUrunService;
            _urunService = urunService;
        }

        public ActionResult Index()
        {
            var model = _icraService.GetIcraWithRelations()
                .Where(i => i.SilTarZmn == null) // Soft delete filtresi
                .ToList();

            ViewBag.MahkemeList = _mahkemeService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null)
                .Select(m => new SelectListItem
                {
                    Value = m.MAHKEME_ID.ToString(),
                    Text = m.MAHKEME_AD
                }).ToList();

            ViewBag.UrunList = _urunService.GetAll()
                .Where(u => u.SIL_TAR_ZMN == null)
                .Select(u => new SelectListItem
                {
                    Value = u.URUN_ID.ToString(),
                    //Text = u.URUN_AD // Daha anlamlı alan gösterimi
                }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(IcraDto model)
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
                var entity = new Icra
                {
                    IHTAR_URUN_ID = model.IhtarUrunId,
                    MAHKEME_ID = model.MahkemeId,
                    ICRA_DOSYA_NO = model.IcraDosyaNo,
                    GRS_TAR_ZMN = DateTime.Now
                };

                _icraService.Add(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetIcra(Guid id)
        {
            var dto = _icraService.GetByIdWithRelations(id);
            if (dto == null)
                return Json(new { success = false, error = "Kayıt bulunamadı" }, JsonRequestBehavior.AllowGet);

            return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(IcraDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var entity = _icraService.GetById(model.IcraId);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                entity.IHTAR_URUN_ID = model.IhtarUrunId;
                entity.MAHKEME_ID = model.MahkemeId;
                entity.ICRA_DOSYA_NO = model.IcraDosyaNo;
                entity.GNC_TAR_ZMN = DateTime.Now;

                _icraService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var entity = _icraService.GetById(id);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                entity.SIL_TAR_ZMN = DateTime.Now;
                _icraService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
