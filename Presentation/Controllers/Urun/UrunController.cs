using Business.Abstract;
using Presentation.Mapping;
using Presentation.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Controllers
{
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
            var model = _urunService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null)
                .Select(m => m.ToModel())
                .ToList();

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
                var entity = model.ToEntity();
                entity.GRS_TAR_ZMN = DateTime.Now;

                _urunService.Add(entity);
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
            var entity = _urunService.GetById(id);
            if (entity == null) return HttpNotFound();

            return Json(entity.ToModel(), JsonRequestBehavior.AllowGet);
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

                model.ApplyTo(entity);
                entity.GNC_TAR_ZMN = DateTime.Now;

                _urunService.Update(entity);
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
                var entity = _urunService.GetById(id);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                // Soft delete 
                entity.SIL_TAR_ZMN = DateTime.Now;
                _urunService.Delete(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}