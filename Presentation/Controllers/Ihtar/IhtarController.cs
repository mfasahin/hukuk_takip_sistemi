using Business.Abstract;
using Entity.Concrete;
using Entity.Dto;
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
        private readonly ISubeService _subeService;
        private readonly IAvukatService _avukatService;
        private readonly IUrunService _urunService;

        // Constructor: DI ile servisler enjekte ediliyor
        public IhtarController(
            IIhtarService ihtarService,
            IMusteriService musteriService,
            ISubeService subeService,
            IAvukatService avukatService,
            IUrunService urunService)
        {
            _ihtarService = ihtarService;
            _musteriService = musteriService;
            _subeService = subeService;
            _avukatService = avukatService;
            _urunService = urunService;
        }

        public ActionResult Index()
        {
            // İlişkili ihtarları getir
            var ihtarList = _ihtarService.GetIhtarWithRelations();

            // Entity → DTO dönüşümü
            var model = ihtarList.Select(i => new IhtarDto
            {
                IhtarId = i.IhtarId,
                BorcTutar = i.BorcTutar,
                IhtarTarih = i.IhtarTarih,

                MusteriId = i.MusteriId,
                MusteriAd = i.MusteriAd,

                AvukatId = i.AvukatId,
                AvukatAd = i.AvukatAd,

                SubeId = i.SubeId,
                SubeAd = i.SubeAd,

                UrunId = i.UrunId,
                UrunAd = i.UrunAd,

                SilTarZmn = i.SilTarZmn
            }).ToList();

            // Dropdown listeleri doldur
            ViewBag.MusteriList = _musteriService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null)
                .Select(m => new SelectListItem
                {
                    Value = m.MUSTERI_ID.ToString(),
                    Text = m.MUST_AD + " " + m.MUST_SOYAD
                }).ToList();

            ViewBag.SubeList = _subeService.GetAll()
                .Select(s => new SelectListItem
                {
                    Value = s.SUBE_ID.ToString(),
                    Text = s.SUBE_ADI
                }).ToList();

            ViewBag.AvukatList = _avukatService.GetAll()
                .Select(a => new SelectListItem
                {
                    Value = a.AVUKAT_ID.ToString(),
                    Text = a.AVKT_AD
                }).ToList();

            ViewBag.UrunList = _urunService.GetAll()
                .Select(u => new SelectListItem
                {
                    Value = u.URUN_ID.ToString(),
                    Text = u.URUN_AD
                }).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult GetIhtar(int id)
        {
            var ihtar = _ihtarService.GetByIdWithRelations(id);   // <-- GetById DEĞİL, bu
            if (ihtar == null) return HttpNotFound();

            var urun = ihtar.IhtarUrunler?.FirstOrDefault();

            var dto = new IhtarDto
            {
                IhtarId = ihtar.IHTAR_ID,
                BorcTutar = ihtar.BORC_TUTAR,
                IhtarTarih = ihtar.IHTAR_TAR_ZMN,
                MusteriId = ihtar.MUSTERI_ID,
                MusteriAd = ihtar.Musteri?.MUST_AD ?? string.Empty,
                AvukatId = ihtar.AVUKAT_ID,
                AvukatAd = ihtar.Avukat?.AVKT_AD ?? string.Empty,
                SubeId = ihtar.SUBE_ID,
                SubeAd = ihtar.Sube?.SUBE_ADI ?? string.Empty,
                UrunId = urun?.URUN_ID ?? 0,
                UrunAd = urun?.Urun?.URUN_AD ?? string.Empty,
                SilTarZmn = ihtar.SIL_TAR_ZMN
            };

            return Json(dto, JsonRequestBehavior.AllowGet);
        }
        //GÜNCELLEME
        [HttpPost]
        public ActionResult UpdateIhtar(IhtarDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var entity = _ihtarService.GetById(model.IhtarId);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                // Alanları DTO’dan entity’ye aktar
                entity.BORC_TUTAR = model.BorcTutar;
                entity.IHTAR_TAR_ZMN = model.IhtarTarih;
                entity.MUSTERI_ID = model.MusteriId;
                entity.AVUKAT_ID = model.AvukatId;
                entity.SUBE_ID = model.SubeId;

                // Ürün güncellemesi (tek ürün için)
                if (model.UrunId > 0)
                {
                    // İlgili ürün ilişkisini güncelle
                    var urunRelation = entity.IhtarUrunler.FirstOrDefault();
                    if (urunRelation != null)
                    {
                        urunRelation.URUN_ID = model.UrunId;
                    }
                    else
                    {
                        entity.IhtarUrunler.Add(new IhtarUrun
                        {
                            URUN_ID = model.UrunId,
                            IHTAR_ID = entity.IHTAR_ID
                        });
                    }
                }

                entity.GNC_TAR_ZMN = DateTime.Now;

                _ihtarService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        //[HttpPost]
        //public ActionResult Create(IhtarModel ihtar)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var errors = ModelState.Values.SelectMany(v => v.Errors)
        //                                      .Select(e => e.ErrorMessage);
        //        return Json(new { success = false, error = string.Join("; ", errors) });
        //    }

        //    try
        //    {
        //        var musteri = new Musteri
        //        {
        //            MUST_NO = ihtar.MustNo,
        //            MUST_AD = ihtar.MustAd,
        //            MUST_SOYAD = ihtar.MustSoyad,
        //            MUST_KIMLIK_NO = ihtar.MustKimlikNo,
        //            MUST_VKN_NO = ihtar.MustVknNo,
        //            MUST_EPOSTA = ihtar.MustEposta,
        //            MUST_TEL_NO = ihtar.MustTelNo,
        //            GRS_TAR_ZMN = DateTime.Now
        //        };

        //        _ihtarService.Add(ihtar);

        //        return Json(new { success = true });
        //    }
        //    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
        //    {
        //        var errors = ex.EntityValidationErrors
        //            .SelectMany(e => e.ValidationErrors)
        //            .Select(e => $"Property: {e.PropertyName}, Error: {e.ErrorMessage}");
        //        return Json(new { success = false, error = string.Join("; ", errors) });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, error = ex.Message });
        //    }


        ////SİLME
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        var entity = _musteriService.GetById(id);
        //        if (entity == null)
        //            return Json(new { success = false, error = "Kayıt bulunamadı" });

        //        // Soft delete → SilinmeTarihi doldur
        //        entity.SIL_TAR_ZMN = DateTime.Now;
        //        _musteriService.Update(entity);

        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, error = ex.Message });
        //    }
        //}

    }
}

