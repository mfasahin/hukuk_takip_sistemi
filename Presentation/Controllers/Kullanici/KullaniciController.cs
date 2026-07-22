using Business.Abstract;
using Presentation.Models;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly IKullaniciService _kullaniciService;

        public KullaniciController(IKullaniciService kullaniciService)
        {
            _kullaniciService = kullaniciService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["KullaniciId"] != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(KullaniciModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Lütfen tüm alanları doldurun";
                return View(model);
            }

            var kullanici = _kullaniciService.Login(model.KullaniciAd, model.Sifre);

            if (kullanici == null)
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı";
                return View(model);
            }

            Session["KullaniciId"] = kullanici.KULLANICI_ID;
            Session["KullaniciAd"] = kullanici.KULLANICI_AD;

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (Session["KullaniciId"] != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(KullaniciRegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, error) = _kullaniciService.Register(model.KullaniciAd, model.Sifre);

            if (!success)
            {
                ViewBag.Error = error;
                return View(model);
            }

            ViewBag.Success = "Kayıt başarılı! Şimdi giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}