using Business.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Dto;
using Presentation.Filters;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    [RequireLogin]
    public class IhtarController : Controller
    {
        private readonly IIhtarService _ihtarService;
        private readonly IMusteriService _musteriService;
        private readonly ISubeService _subeService;
        private readonly IAvukatService _avukatService;
        private readonly IUrunService _urunService;
        private readonly IIhtarUrunService _ihtarUrunService;

        public IhtarController(
            IIhtarService ihtarService,
            IMusteriService musteriService,
            ISubeService subeService,
            IAvukatService avukatService,
            IIhtarUrunService ihtarUrunService,
            IUrunService urunService)
        {
            _ihtarService = ihtarService;
            _musteriService = musteriService;
            _subeService = subeService;
            _avukatService = avukatService;
            _urunService = urunService;
            _ihtarUrunService = ihtarUrunService;
        }

        public ActionResult Index()
        {
            var ihtarListesi = _ihtarService.GetIhtarDto();

            ViewBag.MusteriList = _musteriService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null)
                .Select(m => new SelectListItem
                {
                    Value = m.MUSTERI_ID.ToString(),
                    Text = m.MUST_NO + "-" + m.MUST_AD + " " + m.MUST_SOYAD
                }).ToList();

            ViewBag.SubeList = _subeService.GetAll()
                .Where(s => s.SIL_TAR_ZMN == null)
                .Select(s => new SelectListItem
                {
                    Value = s.SUBE_ID.ToString(),
                    Text = s.SUBE_ADI
                }).ToList();

            ViewBag.AvukatList = _avukatService.GetAll()
                .Where(a => a.SIL_TAR_ZMN == null)
                .Select(a => new SelectListItem
                {
                    Value = a.AVUKAT_ID.ToString(),
                    Text = a.AVKT_AD + " " + a.AVKT_SOYAD
                }).ToList();

            ViewBag.UrunList = _urunService.GetAll()
                .Where(u => u.SIL_TAR_ZMN == null)
                .Select(u => new SelectListItem
                {
                    Value = u.URUN_ID.ToString(),
                    Text = u.URUN_AD
                }).ToList();

            return View(ihtarListesi);
        }

        // EKLEME
        [HttpPost]
        public ActionResult Create(IhtarDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, error = "ModelState geçersiz", details = errors });
            }

            try
            {
                // İhtar kaydı
                var ihtar = new Ihtar
                {
                    IHTAR_ID = Guid.NewGuid(),
                    BORC_TUTAR = model.BorcTutar,
                    IHTAR_TAR_ZMN = model.IhtarTarih,
                    MUSTERI_ID = model.MusteriId,
                    AVUKAT_ID = model.AvukatId,
                    SUBE_ID = model.SubeId,
                };

                _ihtarService.Add(ihtar);

                // Ürün seçilmişse ilişkiyi kaydet
                if (model.UrunId != Guid.Empty)
                {
                    var ihtarUrun = new IhtarUrun
                    {
                        IHTAR_URUN_ID = Guid.NewGuid(),
                        IHTAR_ID = ihtar.IHTAR_ID,
                        URUN_ID = model.UrunId,
                    };

                    _ihtarUrunService.Add(ihtarUrun);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetIhtar(Guid id)
        {
            var ihtarDto = _ihtarService.GetByIdIhtarDto(id);
            if (ihtarDto == null) return HttpNotFound();

            // Eğer ürün ilişkisi varsa, seçili ürünü DTO’ya yaz
            var ihtarUrun = _ihtarUrunService.GetByIhtarId(id).FirstOrDefault();
            if (ihtarUrun != null)
            {
                ihtarDto.UrunId = ihtarUrun.URUN_ID;
            }

            return Json(ihtarDto, JsonRequestBehavior.AllowGet);
        }

        // GÜNCELLEME
        [HttpPost]
        public ActionResult Update(IhtarDto ihtarDto, IhtarUrunDto ihtarUrunDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new
                    {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    })
                    .ToList();

                return Json(new { success = false, error = "ModelState geçersiz", details = errors });
            }

            try
            {
                var IHTAR = _ihtarService.GetById(ihtarDto.IhtarId);
                

                if (IHTAR == null)
                    return Json(new { success = false, error = "İhtar kaydı bulunamadı" });

                // İhtar alanlarını güncelle
                IHTAR.BORC_TUTAR = ihtarDto.BorcTutar;
                IHTAR.IHTAR_TAR_ZMN = ihtarDto.IhtarTarih;
                IHTAR.MUSTERI_ID = ihtarDto.MusteriId;
                IHTAR.AVUKAT_ID = ihtarDto.AvukatId;
                IHTAR.SUBE_ID = ihtarDto.SubeId;

                _ihtarService.Update(IHTAR);

                //var ihtarUrun = _ihtarUrunService.GetById(model.IhtarId);

                var ihtarUrun = _ihtarUrunService.GetById(ihtarUrunDto.IhtarUrunId);

                if (ihtarUrunDto.UrunId != Guid.Empty)
                {
                    if (ihtarUrun != null)
                    {
                        // Güncelle
                        ihtarUrun.URUN_ID = ihtarUrunDto.UrunId;
                        _ihtarUrunService.Update(ihtarUrun);
                    }
                    else
                    {
                        // Yeni ekle
                        _ihtarUrunService.Add(new IhtarUrun
                        {
                            IHTAR_URUN_ID = Guid.NewGuid(),
                            IHTAR_ID = IHTAR.IHTAR_ID,
                            URUN_ID = ihtarUrunDto.UrunId
                        });
                    }
                }
                else
                {
                    // Ürün seçilmemişse mevcut ilişkiyi kaldır
                    if (ihtarUrun != null)
                    _ihtarUrunService.Delete(ihtarUrunDto.IhtarUrunId);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // SİLME (soft delete)
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var ihtar = _ihtarService.GetById(id);
                if (ihtar == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                _ihtarService.Delete(ihtar);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}