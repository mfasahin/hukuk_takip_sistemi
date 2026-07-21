using Business.Abstract;
using Entity.Concrete;
using Presentation.Mapping;
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

        //LİSTELEME
        public ActionResult Index()
        {
            var avukatList = _avukatService.GetAll()
                .Where(m => m.SIL_TAR_ZMN == null)
                .ToList();

            var model = avukatList.Select(m => new AvukatModel
            {
                AvukatId = m.AVUKAT_ID,
                AvktAd = m.AVKT_AD,
                AvktSoyad = m.AVKT_SOYAD,
                TbbSicilNo = m.TBB_SICIL_NO,
                AvktTelNo = m.AVKT_TEL_NO,
                AvktEposta = m.AVKT_EPOSTA,
                HkkBuroAd = m.HKK_BURO_AD,
                HkkBuroAdres = m.HKK_BURO_ADRES,
                OfisTelNo = m.OFIS_TEL_NO
            }).ToList();

            return View(model);
        }

        // EKLEME
        [HttpPost]
        public ActionResult Create(AvukatModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var avukat = new Avukat
                {
                    AVUKAT_ID = Guid.NewGuid(),
                    AVKT_AD = model.AvktAd,
                    AVKT_SOYAD = model.AvktSoyad,
                    TBB_SICIL_NO = model.TbbSicilNo,
                    AVKT_TEL_NO = model.AvktTelNo,
                    AVKT_EPOSTA = model.AvktEposta,
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

        // TEKİL KAYIT (modal doldurma için)
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
                AvktTelNo = avukat.AVKT_TEL_NO,
                AvktEposta = avukat.AVKT_EPOSTA,
                HkkBuroAd = avukat.HKK_BURO_AD,
                HkkBuroAdres = avukat.HKK_BURO_ADRES,
                OfisTelNo = avukat.OFIS_TEL_NO
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// GÜNCELLEME
        [HttpPost]
        public ActionResult Update(AvukatModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "ModelState geçersiz" });

            try
            {
                var avukat = _avukatService.GetById(model.AvukatId);
                if (avukat == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                avukat.AVKT_AD = model.AvktAd;
                avukat.AVKT_SOYAD = model.AvktSoyad;
                avukat.TBB_SICIL_NO = model.TbbSicilNo;
                avukat.AVKT_TEL_NO = model.AvktTelNo;
                avukat.AVKT_EPOSTA = model.AvktEposta;
                avukat.HKK_BURO_AD = model.HkkBuroAd;
                avukat.HKK_BURO_ADRES = model.HkkBuroAdres;
                avukat.OFIS_TEL_NO = model.OfisTelNo;
                avukat.GNC_TAR_ZMN = DateTime.Now;

                _avukatService.Update(avukat);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // SİLME
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var avukat = _avukatService.GetById(id);
                if (avukat == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                // Soft delete 
                avukat.SIL_TAR_ZMN = DateTime.Now;
                _avukatService.Delete(avukat);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}