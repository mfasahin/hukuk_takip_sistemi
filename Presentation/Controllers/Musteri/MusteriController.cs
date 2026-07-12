using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete;
using Entity.Concrete;
using System.Net;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class MusteriController : Controller
    {
        private readonly IMusteriService _musteriService;

        public MusteriController(IMusteriService musteriService)
        {
            _musteriService = musteriService;
        }

         /*public MusteriController() : this(new MusteriManager(new EfMusteriDal()))
         {
         }*/


        // Listeleme
        public ActionResult Index()
        {
            var musteriler = _musteriService.GetAll();
            return View(musteriler);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Musteri musteri)
        {
            _musteriService.Add(musteri);
            return RedirectToAction("Index");
        }

        // Güncelleme POST dan önce GET, EDİT YERİNE UPDATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Musteri musteri)
        {
            if (ModelState.IsValid)
            {
                _musteriService.Update(musteri);
                return RedirectToAction("Index");
            }
            return View(musteri);
        }


        // Silme GET
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var musteri = _musteriService.GetById(id.Value);
            if (musteri == null)
                return HttpNotFound();

            return View(musteri);
        }

        // Silme POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id) // YANLIŞ
        {
            var musteri = _musteriService.GetById(id);
            _musteriService.Delete(musteri);
            return RedirectToAction("Index");
        }


    }
}