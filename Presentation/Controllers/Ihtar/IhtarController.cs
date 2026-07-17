using Business.Abstract;
using Entity.Dto;
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
    //}


    //[HttpGet]
    //public ActionResult GetMusteri(int id)
    //{
    //    var musteri = _musteriService.GetById(id);
    //    if (musteri == null) return HttpNotFound();

    //    var model = new MusteriModel
    //    {
    //        MusteriId = musteri.MUSTERI_ID,
    //        MustNo = musteri.MUST_NO,
    //        MustAd = musteri.MUST_AD,
    //        MustSoyad = musteri.MUST_SOYAD,
    //        MustKimlikNo = musteri.MUST_KIMLIK_NO,
    //        MustVknNo = musteri.MUST_VKN_NO,
    //        MustEposta = musteri.MUST_EPOSTA,
    //        MustTelNo = musteri.MUST_TEL_NO,
    //        //GNC_TAR_ZMN = musteri.GNC_TAR_ZMN
    //    };

    //    return Json(model, JsonRequestBehavior.AllowGet);
    //}

    //// GÜNCELLEME 
    //[HttpPost]
    //public ActionResult Update(MusteriModel model)
    //{
    //    if (!ModelState.IsValid)
    //        return Json(new { success = false, error = "ModelState geçersiz" });

    //    try
    //    {
    //        var entity = _musteriService.GetById(model.MusteriId);
    //        if (entity == null)
    //            return Json(new { success = false, error = "Kayıt bulunamadı" });

    //        entity.MUST_NO = model.MustNo;
    //        entity.MUST_AD = model.MustAd;
    //        entity.MUST_SOYAD = model.MustSoyad;
    //        entity.MUST_KIMLIK_NO = model.MustKimlikNo;
    //        entity.MUST_VKN_NO = model.MustVknNo;
    //        entity.MUST_EPOSTA = model.MustEposta;
    //        entity.MUST_TEL_NO = model.MustTelNo;
    //        entity.GNC_TAR_ZMN = DateTime.Now;

    //        _musteriService.Update(entity);

    //        return Json(new { success = true });
    //    }
    //    catch (Exception ex)
    //    {
    //        return Json(new { success = false, error = ex.Message });
    //    }
    //}

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
