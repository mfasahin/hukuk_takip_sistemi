using Business.Abstract;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Linq;
using System.Web.Mvc;
using Core;

namespace Presentation.Controllers
{
    public class IcraController : Controller
    {
        private readonly IIcraService _icraService;
        private readonly IMahkemeService _mahkemeService;

        public IcraController(IIcraService icraService, IMahkemeService mahkemeService)
        {
            _icraService = icraService;
            _mahkemeService = mahkemeService;
        }

        public ActionResult Index()
        {
            var model = _icraService.GetIcraWithRelations();

            ViewBag.MahkemeList = _mahkemeService.GetAll()
                .Select(m => new SelectListItem
                {
                    Value = m.MAHKEME_ID.ToString(),
                    Text = m.MAHKEME_AD
                }).ToList();

            ViewBag.IhtarUrunList = _icraService.GetIhtarUrun()
                .Select(x => new SelectListItem
                {
                    Value = x.IhtarUrunId.ToString(),
                    Text = x.DisplayText
                }).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult GetIcra(int id)
        {
            var dto = _icraService.GetByIdWithRelations(id);
            if (dto == null) return HttpNotFound();

            return Json(dto, JsonRequestBehavior.AllowGet);
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
                    ICRA_TAKIP_TAR = model.IcraTakipTar,
                    ICRA_DOSYA_NO = model.IcraDosyaNo,
                    GRS_TAR_ZMN = DateTime.Now,
                };

                _icraService.Add(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
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
                entity.ICRA_TAKIP_TAR = model.IcraTakipTar;
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
        public ActionResult Delete(int id)
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