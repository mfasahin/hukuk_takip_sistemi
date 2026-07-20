using Business.Abstract;
using Presentation.Mapping;
using Presentation.Models;
using System;
using System.Linq;
using System.Web.Mvc;

public class MusteriController : Controller
{
    private readonly IMusteriService _musteriService;

    public MusteriController(IMusteriService musteriService)
    {
        _musteriService = musteriService;
    }

    // LİSTELEME
    public ActionResult Index()
    {
        var model = _musteriService.GetAll()
            .Where(m => m.SIL_TAR_ZMN == null)
            .Select(m => m.ToModel())
            .ToList();

        return View(model);
    }

    // EKLEME
    [HttpPost]
    public ActionResult Create(MusteriModel model)
    {
        if (!ModelState.IsValid)
            return Json(new { success = false, error = "ModelState geçersiz" });

        try
        {
            var entity = model.ToEntity();
            entity.GRS_TAR_ZMN = DateTime.Now;

            _musteriService.Add(entity);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    // TEKİL KAYIT (modal doldurma için)
    [HttpGet]
    public ActionResult GetMusteri(Guid id)
    {
        var entity = _musteriService.GetById(id);
        if (entity == null) return HttpNotFound();

        return Json(entity.ToModel(), JsonRequestBehavior.AllowGet);
    }
    // GÜNCELLEME
    [HttpPost]
    public ActionResult Update(MusteriModel model)
    {
        if (!ModelState.IsValid)
            return Json(new { success = false, error = "ModelState geçersiz" });

        try
        {
            var entity = _musteriService.GetById(model.MusteriId);
            if (entity == null)
                return Json(new { success = false, error = "Kayıt bulunamadı" });

            model.ApplyTo(entity);
            entity.GNC_TAR_ZMN = DateTime.Now;

            _musteriService.Update(entity);
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
            var entity = _musteriService.GetById(id);
            if (entity == null)
                return Json(new { success = false, error = "Kayıt bulunamadı" });

            // Soft delete 
            entity.SIL_TAR_ZMN = DateTime.Now;
            _musteriService.Delete(entity);

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }
}