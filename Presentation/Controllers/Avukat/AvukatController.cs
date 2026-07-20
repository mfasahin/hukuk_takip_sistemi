using Business.Abstract;
using Presentation.Mapping;
using Presentation.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class AvukatController : Controller
    {
        private readonly IAvukatService _avukatService;

        public AvukatController(IAvukatService avukatService)
        {
            _avukatService = avukatService;
        }

        //LİSTELEME
        public ActionResult Index()
        {
            var model = _avukatService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null)
                .Select(m => m.ToModel())
                .ToList();

            return View(model);
        }

        // EKLEME
        [HttpPost]
        public ActionResult Create(AvukatModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var entity = model.ToEntity();
                entity.GRS_TAR_ZMN = DateTime.Now;

                _avukatService.Add(entity);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // TEKİL KAYIT (modal doldurma için)
        [HttpGet]
        public ActionResult GetAvukat(Guid id)
        {
            var entity = _avukatService.GetById(id);
            if (entity == null) return HttpNotFound();

            return Json(entity.ToModel(), JsonRequestBehavior.AllowGet);
        }

        /// GÜNCELLEME
        [HttpPost]
        public ActionResult Update(AvukatModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var entity = _avukatService.GetById(model.AvukatId);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                model.ApplyTo(entity);
                entity.GNC_TAR_ZMN = DateTime.Now;

                _avukatService.Update(entity);
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
                var entity = _avukatService.GetById(id);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                // Soft delete 
                entity.SIL_TAR_ZMN = DateTime.Now;
                _avukatService.Delete(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}