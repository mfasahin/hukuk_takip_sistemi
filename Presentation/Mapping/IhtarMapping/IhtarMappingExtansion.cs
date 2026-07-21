//using Entity.Concrete;
//using Presentation.Models;
//using System;
//using System.Linq;

//namespace Presentation.Mappings
//{
//    public static class IhtarMappingExtensions
//    {
//        // Entity → Model
//        public static IhtarModel ToModel(this Ihtar entity)
//        {
//            if (entity == null) return null;

//            return new IhtarModel
//            {
//                IhtarId = entity.IHTAR_ID,
//                MusteriId = entity.MUSTERI_ID,
//                SubeId = entity.SUBE_ID,
//                AvukatId = entity.AVUKAT_ID,
//                UrunId = entity., // ???

//                MusteriAd = entity.Musteri?.MUST_AD,
//                SubeAd = entity.Sube?.SUBE_ADI,
//                AvukatAd = entity.Avukat?.AVKT_AD,
//                //UrunAd = entity.Urun?.UrunAd,

//                BorcTutar = entity.BORC_TUTAR,
//                IhtarTarZmn = entity.IHTAR_TAR_ZMN,

//                // İhtarUrun üzerinden ürünleri çekiyoruz
//                Urunler = entity.IhtarUrunler?
//                    .Select(iu => iu.Urun.ToModel())
//                    .ToList(),

//                SecilenUrunler = entity.IhtarUrunler?
//                    .Select(iu => iu.URUN_ID)
//                    .ToList()
//            };
//        }

//        // Model → Entity
//        public static Ihtar ToEntity(this IhtarModel model)
//        {
//            if (model == null) return null;

//            return new Ihtar
//            {
//                IHTAR_ID = model.IhtarId,
//                MUSTERI_ID = model.MusteriId,
//                SUBE_ID = model.SubeId,
//                AVUKAT_ID = model.AvukatId,
//                UrunId = model.UrunId,

//                BORC_TUTAR = model.BorcTutar,
//                IHTAR_TAR_ZMN = model.IhtarTarZmn,

//                // Seçilen ürünler üzerinden entity listesi oluşturulabilir
//                Urunler = model.SecilenUrunler?.Select(id => new Urun { UrunId = id }).ToList()
//            };
//        }
//    }
//}