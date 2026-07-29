using Business.Abstract;
using Business.Validation;
using Entity.Concrete;
using Entity.Dto;
using Presentation.Filters;
using System;
using System.Linq;
using System.Web.Mvc;
using FluentValidation;

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
        private readonly IIcraService _icraService;

        public IhtarController(
            IIhtarService ihtarService,
            IMusteriService musteriService,
            ISubeService subeService,
            IAvukatService avukatService,
            IIhtarUrunService ihtarUrunService,
            IUrunService urunService,
            IIcraService icraService)
        {
            _ihtarService = ihtarService;
            _musteriService = musteriService;
            _subeService = subeService;
            _avukatService = avukatService;
            _urunService = urunService;
            _ihtarUrunService = ihtarUrunService;
            _icraService = icraService;
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
            if (model == null)
            {
                return Json(new { success = false, message = "Gönderilen ihtar verisi boş olamaz." });
            }

            try
            {
                _ihtarService.Add(model);
                return Json(new { success = true, message = "İhtar başarıyla eklendi." });
            }
            catch (ValidationException ex)
            {
                // "Doğrulama Hataları:" ön eki kaldırıldı, doğrudan mesajlar dönüyor
                var errorList = ex.Errors.Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = errorList });
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return Json(new { success = false, message = msg });
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
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, message = errors });
            }

            try
            {
                var IHTAR = _ihtarService.GetById(ihtarDto.IhtarId);

                if (IHTAR == null)
                    return Json(new { success = false, message = "İhtar kaydı bulunamadı." });

                // İhtar alanlarını güncelle
                IHTAR.BORC_TUTAR = ihtarDto.BorcTutar;
                IHTAR.IHTAR_TAR_ZMN = ihtarDto.IhtarTarih;
                IHTAR.MUSTERI_ID = ihtarDto.MusteriId;
                IHTAR.AVUKAT_ID = ihtarDto.AvukatId;
                IHTAR.SUBE_ID = ihtarDto.SubeId;

                _ihtarService.Update(IHTAR);

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

                return Json(new { success = true, message = "İhtar başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            try
            {
                var ihtar = _ihtarService.GetById(id);
                if (ihtar == null)
                    return Json(new { success = false, message = "Kayıt bulunamadı." });

                var ihtarurun = _ihtarUrunService.GetByIhtarIdTekli(id);
                if (ihtarurun == null)
                    return Json(new { success = false, message = "İhtar-Ürün kaydı bulunamadı." });

                if (_icraService.IhtaraBagliIcraVarMi(ihtarurun.IHTAR_URUN_ID))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Bu ihtara bağlı icra kaydı bulunduğu için silinemez."
                    });
                }

                // Önce ihtara bağlı tüm IhtarUrun kayıtlarını soft delete yap
                var bagliUrunler = _ihtarUrunService.GetByIhtarId(id);
                foreach (var iu in bagliUrunler)
                {
                    iu.SIL_TAR_ZMN = DateTime.Now;
                    _ihtarUrunService.Delete(iu);
                }

                // İhtar kaydını soft delete yap
                ihtar.SIL_TAR_ZMN = DateTime.Now;
                _ihtarService.Delete(ihtar);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Silme sırasında hata: " + ex.Message });
            }
        }
    }
}