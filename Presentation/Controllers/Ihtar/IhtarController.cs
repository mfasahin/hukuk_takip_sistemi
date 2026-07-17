using Business.Abstract;
using Entity.Concrete;
using Presentation.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class IhtarController : Controller
    {
        private readonly IIhtarService _ihtarService;
        private readonly IMusteriService _musteriService;
        private readonly IAvukatService _avukatService;
        private readonly ISubeService _subeService;
        private readonly IIhtarUrunService _ihtarUrunService;
        private readonly IUrunService _urunService;


        // Constructor: DI ile IMusteriService enjekte ediliyor
        public IhtarController(
            IIhtarService ihtarService,
            IMusteriService musteriService,
            IAvukatService avukatService,
            ISubeService subeService,
            IIhtarUrunService ihtarUrunService,
            IUrunService urunService)
        {
            _ihtarService = ihtarService;
            _musteriService = musteriService;
            _avukatService = avukatService;
            _subeService = subeService;
            _ihtarUrunService = ihtarUrunService;
            _urunService = urunService;
        }

        public ActionResult Index()
        {
            var ihtarList = _ihtarService.GetIhtarWithRelations();

            var model = ihtarList.Select(i => new IhtarModel
            {
                IhtarId = i.IHTAR_ID,
                MusteriAd = i.Musteri.MUST_AD,
                BorcTutar = i.BORC_TUTAR,
                IhtarTarZmn = i.IHTAR_TAR_ZMN,
                SubeAd = i.Sube.SUBE_ADI,
                AvukatAd = i.Avukat.AVKT_AD,

                // Ürünler artık List<UrunModel>
                Urunler = i.IhtarUrunler.Select(u => new UrunModel
                {
                    UrunId = u.URUN_ID,
                    UrunAd = u.Urun.URUN_AD
                }).ToList()
            }).ToList();


            // Dropdown listeleri doldur
            ViewBag.MusteriList = _musteriService.GetAll()
                .Select(m => new SelectListItem { Value = m.MUSTERI_ID.ToString(), Text = m.MUST_AD })
                .ToList();

            ViewBag.AvukatList = _avukatService.GetAll()
                .Select(a => new SelectListItem { Value = a.AVUKAT_ID.ToString(), Text = a.AVKT_AD })
                .ToList();

            ViewBag.SubeList = _subeService.GetAll()
                .Select(s => new SelectListItem { Value = s.SUBE_ID.ToString(), Text = s.SUBE_ADI })
                .ToList();

            ViewBag.UrunList = _urunService.GetAll()
                .Select(u => new UrunModel { UrunId = u.URUN_ID, UrunAd = u.URUN_AD })
                .ToList();

            return View(model);
        }


        [HttpPost]
        public ActionResult Create(IhtarModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, error = "Geçersiz model", details = errors });
            }

            try
            {
                var ihtar = new Ihtar
                {
                    BORC_TUTAR = model.BorcTutar,
                    IHTAR_TAR_ZMN = model.IhtarTarZmn,
                    MUSTERI_ID = model.MusteriId,   // MusteriAd yerine MusteriId kullanılmalı
                    SUBE_ID = model.SubeId,
                    AVUKAT_ID = model.AvukatId,
                    GRS_TAR_ZMN = DateTime.Now
                };

                _ihtarService.Add(ihtar);

                // Ürünleri ihtar_ürün tablosuna ekle
                if (model.SecilenUrunler != null && model.SecilenUrunler.Any())
                {
                    foreach (var urunId in model.SecilenUrunler)
                    {
                        var ihtarUrun = new IhtarUrun
                        {
                            IHTAR_ID = ihtar.IHTAR_ID,
                            URUN_ID = urunId
                        };
                        _ihtarUrunService.Add(ihtarUrun);
                    }
                }

                // İsimleri doldur
                var musteriAd = _musteriService.GetById(model.MusteriId)?.MUST_AD;
                var subeAd = _subeService.GetById(model.SubeId)?.SUBE_ADI;
                var avukatAdSoyad = _avukatService.GetById(model.AvukatId)?.AVKT_AD;

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        ihtarId = ihtar.IHTAR_ID,
                        borcTutar = ihtar.BORC_TUTAR,
                        ihtarTarZmn = ihtar.IHTAR_TAR_ZMN.ToString("dd.MM.yyyy"),
                        musteriAd = musteriAd,
                        subeAd = subeAd,
                        avukatAdSoyad = avukatAdSoyad
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Sunucu hatası", details = ex.Message });
            }
        }



        [HttpGet]
        public ActionResult GetIhtar(int id)
        {
            var ihtar = _ihtarService.GetById(id);
            if (ihtar == null) return HttpNotFound();

            var model = new IhtarModel
            {
                IhtarId = ihtar.IHTAR_ID,
                BorcTutar = ihtar.BORC_TUTAR,
                IhtarTarZmn = ihtar.IHTAR_TAR_ZMN,
                //GNC_TAR_ZMN = musteri.GNC_TAR_ZMN
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var entity = _ihtarService.GetById(id);
            if (entity == null)
                return HttpNotFound();

            var model = new IhtarModel
            {
                IhtarId = entity.IHTAR_ID,
                BorcTutar = entity.BORC_TUTAR,
                IhtarTarZmn = entity.IHTAR_TAR_ZMN,
                MusteriId = entity.MUSTERI_ID,   // <-- bu satırlar eksikse
                AvukatId = entity.AVUKAT_ID,    // <-- dropdown boş kalır
                SubeId = entity.SUBE_ID
            };

            // Create action'ındaki ile AYNI şekilde dolduruluyor olmalı
            ViewBag.MusteriList = _musteriService.GetAll()
                .Select(m => new SelectListItem { Value = m.MUSTERI_ID.ToString(), Text = m.MUST_AD + " " + m.MUST_SOYAD })
                .ToList();

            ViewBag.SubeList = _subeService.GetAll()
                .Select(s => new SelectListItem { Value = s.SUBE_ID.ToString(), Text = s.SUBE_ADI })
                .ToList();

            ViewBag.AvukatList = _avukatService.GetAll()
                .Select(a => new SelectListItem { Value = a.AVUKAT_ID.ToString(), Text = a.AVKT_AD + " " + a.AVKT_SOYAD })
                .ToList();

            return PartialView("_UpdatePartial", model);
        }

        //SİLME
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var entity = _ihtarService.GetById(id);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                // Soft delete → SilinmeTarihi doldur
                entity.SIL_TAR_ZMN = DateTime.Now;
                _ihtarService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}