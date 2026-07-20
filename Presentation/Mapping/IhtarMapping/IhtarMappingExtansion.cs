using Entity.Concrete;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Mapping
{
    public static class IhtarMappingExtensions
    {
        // Entity -> Model (listeleme ve tekil kayıt GET için)
        // NOT: entity'nin Musteri, Avukat, Sube, IhtarUrunler.Urun ile birlikte
        // (Include edilmiş) gelmesi gerekir, aksi halde ObjectDisposedException alınır.
        public static IhtarModel ToModel(this Ihtar entity)
        {
            if (entity == null) return null;

            return new IhtarModel
            {
                IhtarId = entity.IHTAR_ID,
                MusteriId = entity.MUSTERI_ID,
                SubeId = entity.SUBE_ID,
                AvukatId = entity.AVUKAT_ID,

                MusteriAd = entity.Musteri?.MUST_AD + " " + entity.Musteri?.MUST_SOYAD,
                SubeAd = entity.Sube?.SUBE_ADI ?? string.Empty,
                AvukatAd = entity.Avukat?.AVKT_AD ?? string.Empty,

                BorcTutar = entity.BORC_TUTAR,
                IhtarTarZmn = entity.IHTAR_TAR_ZMN,

                Urunler = entity.IhtarUrunler?
                    .Where(iu => iu.Urun != null)
                    .Select(iu => new UrunModel
                    {
                        UrunId = iu.Urun.URUN_ID,
                        UrunAd = iu.Urun.URUN_AD
                    }).ToList() ?? new List<UrunModel>(),

                SecilenUrunler = entity.IhtarUrunler?
                    .Select(iu => iu.URUN_ID)
                    .ToList() ?? new List<Guid>()
            };
        }

        // Skaler + FK alanları entity'ye uygular (Update'te de, Create'te de kullanılır)
        public static void ApplyTo(this IhtarModel model, Ihtar entity)
        {
            entity.MUSTERI_ID = model.MusteriId;
            entity.SUBE_ID = model.SubeId;
            entity.AVUKAT_ID = model.AvukatId;
            entity.BORC_TUTAR = model.BorcTutar;
            entity.IHTAR_TAR_ZMN = model.IhtarTarZmn;
        }

        // Yeni Ihtar + bağlı IhtarUrun kayıtlarını birlikte oluşturur (Create için)
        public static Ihtar ToEntity(this IhtarModel model)
        {
            var entity = new Ihtar
            {
                IHTAR_ID = Guid.NewGuid(),
                GRS_TAR_ZMN = DateTime.Now
            };

            model.ApplyTo(entity);

            entity.IhtarUrunler = new List<IhtarUrun>();
            if (model.SecilenUrunler != null)
            {
                foreach (var urunId in model.SecilenUrunler.Distinct())
                {
                    entity.IhtarUrunler.Add(new IhtarUrun
                    {
                        IHTAR_URUN_ID = Guid.NewGuid(),
                        IHTAR_ID = entity.IHTAR_ID,
                        URUN_ID = urunId,
                        GRS_TAR_ZMN = DateTime.Now
                    });
                }
            }

            return entity;
        }

        // Update'te: seçilenlerden entity'de olmayanları ekler, artık seçilmeyenleri kaldırır.
        public static void SyncUrunler(this IhtarModel model, Ihtar entity)
        {
            if (entity.IhtarUrunler == null)
                entity.IhtarUrunler = new List<IhtarUrun>();

            var secilenler = model.SecilenUrunler ?? new List<Guid>();

            // Artık seçilmeyenleri kaldır
            var kaldirilacaklar = entity.IhtarUrunler
                .Where(iu => !secilenler.Contains(iu.URUN_ID))
                .ToList();
            foreach (var k in kaldirilacaklar)
                entity.IhtarUrunler.Remove(k);

            // Yeni seçilenleri ekle
            var mevcutUrunIdler = new HashSet<Guid>(entity.IhtarUrunler.Select(x => x.URUN_ID));
            foreach (var urunId in secilenler.Distinct())
            {
                if (!mevcutUrunIdler.Contains(urunId))
                {
                    entity.IhtarUrunler.Add(new IhtarUrun
                    {
                        IHTAR_URUN_ID = Guid.NewGuid(),
                        IHTAR_ID = entity.IHTAR_ID,
                        URUN_ID = urunId,
                        GRS_TAR_ZMN = DateTime.Now
                    });
                }
            }
        }
    }
}