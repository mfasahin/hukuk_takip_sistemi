using System;
using System.Linq;
using Entity.Concrete;
using Presentation.Models;

namespace Presentation.Mapping
{
    public static class IhtarMappingExtansion
    {
        // Entity -> Model
        public static IhtarModel ToModel(this Ihtar entity)
        {
            if (entity == null) return null;

            return new IhtarModel
            {
                IhtarId = entity.IHTAR_ID,
                MusteriId = entity.MUSTERI_ID,
                SubeId = entity.SUBE_ID,
                AvukatId = entity.AVUKAT_ID,
                UrunId = entity.URUN_ID,

                MusteriAd = entity.Musteri?.MUSTERI_AD,
                SubeAd = entity.Sube?.SUBE_AD,
                AvukatAd = entity.Avukat?.AVUKAT_AD,
                UrunAd = entity.Urun?.URUN_AD,

                BorcTutar = entity.BORC_TUTAR,
                IhtarTarZmn = entity.IHTAR_TAR_ZMN,

                // Navigation: ürünler
                Urunler = entity.Urunler?.Select(u => u.ToModel()).ToList(),

                // Çoklu seçim için
                SecilenUrunler = entity.Urunler?.Select(u => u.URUN_ID).ToList()
            };
        }

        // Model -> Entity (Update için)
        public static void ApplyTo(this IhtarModel model, Ihtar entity)
        {
            entity.MUSTERI_ID = model.MusteriId;
            entity.SUBE_ID = model.SubeId;
            entity.AVUKAT_ID = model.AvukatId;
            entity.URUN_ID = model.UrunId;

            entity.BORC_TUTAR = model.BorcTutar;
            entity.IHTAR_TAR_ZMN = model.IhtarTarZmn;

            // Seçilen ürünler üzerinden entity listesi güncellenebilir
            if (model.SecilenUrunler != null)
            {
                entity.Urunler = model.SecilenUrunler
                    .Select(id => new Urun { URUN_ID = id })
                    .ToList();
            }
        }

        // Model -> Entity (Create için)
        public static Ihtar ToEntity(this IhtarModel model)
        {
            var entity = new Ihtar { IHTAR_ID = Guid.NewGuid() };
            model.ApplyTo(entity);
            return entity;
        }
    }
}
