using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;


using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class AvukatController : Controller
    {
        AvukatManager _avukatManager = new AvukatManager(new EfAvukatDal());

        public ActionResult Index()
        {
            var avukatlar = _avukatManager.GetAll();
            return View(avukatlar);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Avukat avukat)
        {
            _avukatManager.Add(avukat);
            return RedirectToAction("Index");
        }
    }
}