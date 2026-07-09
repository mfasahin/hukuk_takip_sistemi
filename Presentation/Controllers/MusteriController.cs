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
    public class MusteriController : Controller
    {
        MusteriManager _musteriManager = new MusteriManager(new EfMusteriDal());

        // Listeleme
        public ActionResult Index()
        {
            var musteriler = _musteriManager.GetAll();
            return View(musteriler);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Musteri musteri)
        {
            _musteriManager.Add(musteri);
            return RedirectToAction("Index");
        }

        // Güncelleme POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Musteri musteri)
        {
            if (ModelState.IsValid)
            {
                _musteriManager.Update(musteri);
                return RedirectToAction("Index");
            }
            return View(musteri);
        }


        // Silme GET
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var musteri = _musteriManager.GetById(id.Value);
            if (musteri == null)
                return HttpNotFound();

            return View(musteri);
        }

        // Silme POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var musteri = _musteriManager.GetById(id);
            _musteriManager.Delete(musteri);
            return RedirectToAction("Index");
        }


    }
}