using Entity.Concrete;
using Presentation.Models;
using System;

namespace Presentation.Mappings
{
    public static class IhtarMappingExtensions
    {
        // Entity → Model
        public static IhtarModel ToModel(this Ihtar entity)
        {
            if (entity == null) return null;

            return new IhtarModel
            {
                IhtarId = entity.IHTAR_ID,
                MusteriId = entity.MUSTERI_ID,
                SubeId = entity.SUBE_ID,
                AvukatId = entity.AVUKAT_ID,
                UrunId = entity.UrunId, // ???

                MusteriAd = entity.Musteri?.MUST_AD,
                SubeAd = entity.Sube?.SUBE_ADI,
                AvukatAd = entity.Avukat?.AVKT_AD,
                UrunAd = entity.Urun?.UrunAd,

                BorcTutar = entity.BORC_TUTAR,
                IhtarTarZmn = entity.IHTAR_TAR_ZMN,

                // Navigation: ürünler
                Urunler = entity.Urunler?.Select(u => u.ToModel()).ToList(),

                // Seçilen ürünler (çoklu seçim için)
                SecilenUrunler = entity.Urunler?.Select(u => u.UrunId).ToList()
            };
        }

        // Model → Entity
        public static Ihtar ToEntity(this IhtarModel model)
        {
            if (model == null) return null;

            return new Ihtar
            {
                IHTAR_ID = model.IhtarId,
                MUSTERI_ID = model.MusteriId,
                SUBE_ID = model.SubeId,
                AVUKAT_ID = model.AvukatId,
                UrunId = model.UrunId,

                BORC_TUTAR = model.BorcTutar,
                IHTAR_TAR_ZMN = model.IhtarTarZmn,

                // Seçilen ürünler üzerinden entity listesi oluşturulabilir
                Urunler = model.SecilenUrunler?.Select(id => new Urun { UrunId = id }).ToList()
            };
        }
    }
}