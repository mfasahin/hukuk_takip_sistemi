using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Concrete.EntityFramework;

using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class MusteriController : Controller
    {
        MusteriManager _musteriManager = new MusteriManager(new EfMusteriDal());

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
    }
}