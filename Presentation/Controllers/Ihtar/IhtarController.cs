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
            var dtoList = _ihtarService.GetIhtarWithRelations();

            var modelList = dtoList.Select(dto => new IhtarModel
            {
                IhtarId = dto.IhtarId,
                BorcTutar = dto.BorcTutar,
                //IhtarTarZmn = dto.IhtarTarih,
                MusteriAd = dto.MusteriAd,
                AvukatAd = dto.AvukatAd,
                SubeAd = dto.SubeAd
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
                .Select(u => new SelectListItem { Value = u.URUN_ID.ToString(), Text = u.URUN_AD })
                .ToList();

            return View(modelList);
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
            var entity = _ihtarService.GetById(id);
            if (entity == null)
                return HttpNotFound();

            var model = new IhtarModel
            {
                IhtarId = entity.IHTAR_ID,
                BorcTutar = entity.BORC_TUTAR,
                IhtarTarZmn = entity.IHTAR_TAR_ZMN,
                MusteriId = entity.MUSTERI_ID,
                AvukatId = entity.AVUKAT_ID,
                SubeId = entity.SUBE_ID,
                SecilenUrunler = entity.IhtarUrunler?.Select(u => u.URUN_ID).ToList()
            };

            // Dropdown listeleri doldur
            ViewBag.MusteriList = _musteriService.GetAll()
                .Select(m => new SelectListItem { Value = m.MUSTERI_ID.ToString(), Text = m.MUST_AD + " " + m.MUST_SOYAD, Selected = (m.MUSTERI_ID == model.MusteriId) })
                .ToList();

            ViewBag.SubeList = _subeService.GetAll()
                .Select(s => new SelectListItem { Value = s.SUBE_ID.ToString(), Text = s.SUBE_ADI, Selected = (s.SUBE_ID == model.SubeId) })
                .ToList();

            ViewBag.AvukatList = _avukatService.GetAll()
                .Select(a => new SelectListItem { Value = a.AVUKAT_ID.ToString(), Text = a.AVKT_AD + " " + a.AVKT_SOYAD, Selected = (a.AVUKAT_ID == model.AvukatId) })
                .ToList();

            ViewBag.UrunList = _urunService.GetAll()
                .Select(u => new SelectListItem { Value = u.URUN_ID.ToString(), Text = u.URUN_AD, Selected = model.SecilenUrunler != null && model.SecilenUrunler.Contains(u.URUN_ID) })
                .ToList();

            return PartialView("_UpdatePartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(IhtarModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Model geçersiz" });

            var entity = _ihtarService.GetById(model.IhtarId);
            if (entity == null)
                return HttpNotFound();

            // Alanları güncelle
            entity.BORC_TUTAR = model.BorcTutar;
            entity.IHTAR_TAR_ZMN = model.IhtarTarZmn;
            entity.MUSTERI_ID = model.MusteriId;
            entity.SUBE_ID = model.SubeId;
            entity.AVUKAT_ID = model.AvukatId;

            _ihtarService.Update(entity);

            // Eski ürün ilişkilerini sil
            //_ihtarUrunService.Delete(ihtar);

            // Yeni seçilen ürünleri ekle
            if (model.SecilenUrunler != null && model.SecilenUrunler.Any())
            {
                foreach (var urunId in model.SecilenUrunler)
                {
                    var ihtarUrun = new IhtarUrun
                    {
                        IHTAR_ID = entity.IHTAR_ID,
                        URUN_ID = urunId
                    };
                    _ihtarUrunService.Add(ihtarUrun);
                }
            }

            return Json(new { success = true });
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