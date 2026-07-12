using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;


using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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

        /*public AvukatController() : this(new MusteriManager(new EfMusteriDal()))
        {
        }*/


        // Listeleme
        public ActionResult Index()
        {
            var avukatlar = _avukatService.GetAll();
            return View(avukatlar);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Avukat avukat)
        {
            _avukatService.Add(avukat);
            return RedirectToAction("Index");
        }

        // Güncelleme POST dan önce GET, EDİT YERİNE UPDATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Avukat avukat)
        {
            if (ModelState.IsValid)
            {
                _avukatService.Update(avukat);
                return RedirectToAction("Index");
            }
            return View(avukat);
        }


        // Silme GET
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var avukat = _avukatService.GetById(id.Value);
            if (avukat == null)
                return HttpNotFound();

            return View(avukat);
        }

        // Silme POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id) // YANLIŞ
        {
            var avukat = _avukatService.GetById(id);
            _avukatService.Delete(avukat);
            return RedirectToAction("Index");
        }
    }
}