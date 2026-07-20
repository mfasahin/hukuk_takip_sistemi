using Business.Abstract;
using Presentation.Models;
using Presentation.Mapping;
using System;
using System.Web.Mvc;
using System.Linq;

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
            .Select(m => m.ToModel())      // <-- extension method çağrısı
            .ToList();

        return View(model);
    }

    // TEKİL KAYIT (modal doldurma için)
    [HttpGet]
    public ActionResult GetMusteri(Guid id)
    {
        var entity = _musteriService.GetById(id);
        if (entity == null) return HttpNotFound();

        return Json(entity.ToModel(), JsonRequestBehavior.AllowGet);   // <-- burada da
    }

    // EKLEME
    [HttpPost]
    public ActionResult Create(MusteriModel model)
    {
        if (!ModelState.IsValid)
            return Json(new { success = false, error = "ModelState geçersiz" });

        try
        {
            var entity = model.ToEntity();          // <-- burada da
            entity.GRS_TAR_ZMN = DateTime.Now;

            _musteriService.Add(entity);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
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

            model.ApplyTo(entity);                  // <-- burada da
            entity.GNC_TAR_ZMN = DateTime.Now;

            _musteriService.Update(entity);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }
}