using Business.Abstract;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class IcraController : Controller
    {
        private readonly IIcraService _icraService;
        private readonly IMusteriService _musteriService;
        private readonly IAvukatService _avukatService;
        private readonly IMahkemeService _mahkemeService;
        private readonly IUrunService _urunService;

        public IcraController(
            IIcraService icraService,
            IMusteriService musteriService,
            IMahkemeService mahkemeService,
            IAvukatService avukatService,
            IUrunService urunService)
        {
            _icraService = icraService;
            _musteriService = musteriService;
            _mahkemeService = mahkemeService;
            _urunService = urunService;
            _avukatService = avukatService;
        }

        public ActionResult Index()
        {
            var model = _icraService.GetIcraWithRelations();

            ViewBag.MusteriList = _musteriService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null)
                .Select(m => new SelectListItem
                {
                    Value = m.MUSTERI_ID.ToString(),
                    Text = m.MUST_AD + " " + m.MUST_SOYAD
                }).ToList();

            ViewBag.AvukatList = _avukatService.GetAll()
                .Where(a => a.SIL_TAR_ZMN == null)
                .Select(a => new SelectListItem
                {
                    Value = a.AVUKAT_ID.ToString(),
                    Text = a.AVKT_AD + " " + a.AVKT_SOYAD
                }).ToList();

            ViewBag.MahkemeList = _mahkemeService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null)
                .Select(m => new SelectListItem
                {
                    Value = m.MAHKEME_ID.ToString(),
                    Text = m.MAHKEME_AD
                }).ToList();

            // İcra bir IhtarUrun'a bağlanıyor - dropdown'da hangi ihtar/ürün olduğu görünmeli
            ViewBag.IhtarUrunList = _icraService.GetIhtarUrun()
                .Select(x => new SelectListItem
                {
                    Value = x.IhtarUrunId.ToString(),
                    Text = x.MusteriAd + " - " + x.UrunAd + " - " + x.IhtarTarih.ToString("dd.MM.yyyy")
                }).ToList();

            return View(model);
        }

        [HttpGet]
        public JsonResult GetIhtarUrunByMusteri(Guid musteriId)
        {
            var list = _icraService.GetIhtarUrunByMusteri(musteriId)
                .Select(x => new
                {
                    value = x.IhtarUrunId,
                    text = x.UrunAd + " - " + x.IhtarTarih.ToString("dd.MM.yyyy")
                });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(IcraDto model)
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
                var entity = new Icra
                {
                    ICRA_ID = Guid.NewGuid(),
                    IHTAR_URUN_ID = model.IhtarUrunId,
                    MAHKEME_ID = model.MahkemeId,
                    ICRA_DOSYA_NO = model.IcraDosyaNo,
                    ICRA_TAKIP_TAR = model.IcraTakipTar,
                    GRS_TAR_ZMN = DateTime.Now
                };

                _icraService.Add(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetIcra(Guid id)
        {
            var dto = _icraService.GetByIdWithRelations(id);
            if (dto == null) return HttpNotFound();

            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(IcraDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var entity = _icraService.GetById(model.IcraId);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                entity.IHTAR_URUN_ID = model.IhtarUrunId;
                entity.MAHKEME_ID = model.MahkemeId;
                entity.ICRA_DOSYA_NO = model.IcraDosyaNo;
                entity.ICRA_TAKIP_TAR = model.IcraTakipTar;
                entity.GNC_TAR_ZMN = DateTime.Now;

                _icraService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var entity = _icraService.GetById(id);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                entity.SIL_TAR_ZMN = DateTime.Now;
                _icraService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}