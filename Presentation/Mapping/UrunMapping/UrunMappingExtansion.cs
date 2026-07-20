using Entity.Concrete;
using Presentation.Models;
using System;

namespace Presentation.Mapping
{
    public static class UrunMappingExtansion
    {
        public static UrunModel ToModel(this Urun entity) // Entity -> Model
        {
            if (entity == null) return null;

            return new UrunModel
            {
                UrunId = entity.URUN_ID,
                UrunAd = entity.URUN_AD,
                UrunKod = entity.URUN_KOD,
                SonGecerlilikTar = entity.SON_GECERLILIK_TAR,
            };
        }
        public static void ApplyTo(this UrunModel model, Urun entity) // Model -> Entity Update için
        {
            entity.URUN_AD = model.UrunAd;
            entity.URUN_KOD = model.UrunKod;
            entity.SON_GECERLILIK_TAR = model.SonGecerlilikTar;
        }
        public static Urun ToEntity(this UrunModel model) // Model -> Entity Create için
        {
            var entity = new Urun { URUN_ID = Guid.NewGuid() };
            model.ApplyTo(entity);
            return entity;
        }
    }
}