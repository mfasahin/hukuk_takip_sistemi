using Business.Abstract;
using Entity.Concrete;
using Presentation.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using static Unity.Storage.RegistrationSet;

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
                AVUKAT_ID = a.AVUKAT_ID,
                AVKT_AD = a.AVKT_AD,
                AVKT_SOYAD = a.AVKT_SOYAD,
                TBB_SICIL_NO = a.TBB_SICIL_NO,
                AVKT_EPOSTA = a.AVKT_EPOSTA,
                AVKT_TEL_NO = a.AVKT_TEL_NO,
                HKK_BURO_AD = a.HKK_BURO_AD,
                HKK_BURO_ADRES = a.HKK_BURO_ADRES,
                OFIS_TEL_NO = a.OFIS_TEL_NO,
                SIL_TAR_ZMN = a.SIL_TAR_ZMN
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
                    AVUKAT_ID = model.AVUKAT_ID,
                    AVKT_AD = model.AVKT_AD,
                    AVKT_SOYAD = model.AVKT_SOYAD,
                    TBB_SICIL_NO = model.TBB_SICIL_NO,
                    AVKT_EPOSTA = model.AVKT_EPOSTA,
                    AVKT_TEL_NO = model.AVKT_TEL_NO,
                    HKK_BURO_AD = model.HKK_BURO_AD,
                    HKK_BURO_ADRES = model.HKK_BURO_ADRES,
                    OFIS_TEL_NO = model.OFIS_TEL_NO,
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
        public ActionResult GetAvukat(int id)
        {
            var avukat = _avukatService.GetById(id);
            if (avukat == null) return HttpNotFound();

            var model = new AvukatModel
            {
                AVUKAT_ID = avukat.AVUKAT_ID,
                AVKT_AD = avukat.AVKT_AD,
                AVKT_SOYAD = avukat.AVKT_SOYAD,
                TBB_SICIL_NO = avukat.TBB_SICIL_NO,
                AVKT_EPOSTA = avukat.AVKT_EPOSTA,
                AVKT_TEL_NO = avukat.AVKT_TEL_NO,
                HKK_BURO_AD = avukat.HKK_BURO_AD,
                HKK_BURO_ADRES = avukat.HKK_BURO_ADRES,
                OFIS_TEL_NO = avukat.OFIS_TEL_NO
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
                var entity = _avukatService.GetById(model.AVUKAT_ID);
                if (entity == null)
                    return Json(new { success = false, error = "Kayıt bulunamadı" });

                entity.AVKT_AD = model.AVKT_AD;
                entity.AVKT_SOYAD = model.AVKT_SOYAD;
                entity.TBB_SICIL_NO = model.TBB_SICIL_NO;
                entity.AVKT_EPOSTA = model.AVKT_EPOSTA;
                entity.AVKT_TEL_NO = model.AVKT_TEL_NO;
                entity.HKK_BURO_AD = model.HKK_BURO_AD;
                entity.HKK_BURO_ADRES = model.HKK_BURO_ADRES;
                entity.OFIS_TEL_NO = model.OFIS_TEL_NO;
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
        public ActionResult Delete(int id)
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