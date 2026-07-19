using Business.Abstract;
using Entity.Concrete;
using Presentation.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class AvukatController : Controller
    {
        private readonly IAvukatService _avukatService;

        public AvukatController(IAvukatService avukatService)
        {
            _avukatService = avukatService;
        }

        public ActionResult Index()
        {
            // Tüm avukatları getir
            var avukatList = _avukatService.GetAll(); // List<Entity.Concrete.Avukat>

            // SilinmeTarihi dolu olanları filtrele (yani silinmişleri gösterme)
            var aktifAvukatlar = avukatList
                .Where(a => a.SIL_TAR_ZMN == null) // sadece silinmemiş kayıtlar
                .ToList();

            // Entity → ViewModel dönüşümü
            var model = aktifAvukatlar.Select(a => new Presentation.Models.AvukatModel
            {
                AvukatId = a.AVUKAT_ID,
                AvktAd = a.AVKT_AD,
                AvktSoyad = a.AVKT_SOYAD,
                TbbSicilNo = a.TBB_SICIL_NO,
                AvktEposta = a.AVKT_EPOSTA,
                AvktTelNo = a.AVKT_TEL_NO,
                HkkBuroAd = a.HKK_BURO_AD,
                HkkBuroAdres = a.HKK_BURO_ADRES,
                OfisTelNo = a.OFIS_TEL_NO,
                SilTarZmn = a.SIL_TAR_ZMN
            }).ToList();

            return View(model); // IEnumerable<AvukatModel> gönderiyoruz
        }
        [HttpPost]
        public ActionResult Create(AvukatModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, error = "Geçersiz model" });
            }

            try
            {
                var avukat = new Avukat
                {
                    AVUKAT_ID = model.AvukatId,
                    AVKT_AD = model.AvktAd,
                    AVKT_SOYAD = model.AvktSoyad,
                    TBB_SICIL_NO = model.TbbSicilNo,
                    AVKT_EPOSTA = model.AvktEposta,
                    AVKT_TEL_NO = model.AvktTelNo,
                    HKK_BURO_AD = model.HkkBuroAd,
                    HKK_BURO_ADRES = model.HkkBuroAdres,
                    OFIS_TEL_NO = model.OfisTelNo,
                    GRS_TAR_ZMN = DateTime.Now
                };

                _avukatService.Add(avukat);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        //lİSTELEME
        [HttpGet]
        public ActionResult GetAvukat(Guid id)
        {
            var avukat = _avukatService.GetById(id);
            if (avukat == null) return HttpNotFound();

            var model = new AvukatModel
            {
                AvukatId = avukat.AVUKAT_ID,
                AvktAd = avukat.AVKT_AD,
                AvktSoyad = avukat.AVKT_SOYAD,
                TbbSicilNo = avukat.TBB_SICIL_NO,
                AvktEposta = avukat.AVKT_EPOSTA,
                AvktTelNo = avukat.AVKT_TEL_NO,
                HkkBuroAd = avukat.HKK_BURO_AD,
                HkkBuroAdres = avukat.HKK_BURO_ADRES,
                OfisTelNo = avukat.OFIS_TEL_NO
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //GÜNCELLEME
        [HttpPost]
        public ActionResult Update(AvukatModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var entity = _avukatService.GetById(model.AvukatId);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                entity.AVKT_AD = model.AvktAd;
                entity.AVKT_SOYAD = model.AvktSoyad;
                entity.TBB_SICIL_NO = model.TbbSicilNo;
                entity.AVKT_EPOSTA = model.AvktEposta;
                entity.AVKT_TEL_NO = model.AvktTelNo;
                entity.HKK_BURO_AD = model.HkkBuroAd;
                entity.HKK_BURO_ADRES = model.HkkBuroAdres;
                entity.OFIS_TEL_NO = model.OfisTelNo;
                entity.GNC_TAR_ZMN = DateTime.Now;

                _avukatService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var entity = _avukatService.GetById(id);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                // Soft delete → SilinmeTarihi doldur
                entity.SIL_TAR_ZMN = DateTime.Now;
                _avukatService.Update(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}