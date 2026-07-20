using Entity.Concrete;
using Presentation.Models;
using System;

namespace Presentation.Mapping
{
    public static class AvukatMappingExtansion
    {
        public static AvukatModel ToModel(this Avukat entity) // Entity -> Model
        {
            if (entity == null) return null;

            return new AvukatModel
            {
                AvukatId = entity.AVUKAT_ID,
                AvktAd= entity.AVKT_AD,
                AvktSoyad = entity.AVKT_SOYAD,
                TbbSicilNo = entity.TBB_SICIL_NO,
                AvktEposta = entity.AVKT_EPOSTA,
                AvktTelNo = entity.AVKT_TEL_NO,
                HkkBuroAd = entity.HKK_BURO_AD,
                HkkBuroAdres = entity.HKK_BURO_ADRES,
                OfisTelNo = entity.OFIS_TEL_NO
            };
        }
        public static void ApplyTo(this AvukatModel model, Avukat entity) // Model -> Entity Update için
        {
            entity.AVKT_AD = model.AvktAd;
            entity.AVKT_SOYAD = model.AvktSoyad;
            entity.TBB_SICIL_NO = model.TbbSicilNo;
            entity.AVKT_EPOSTA = model.AvktEposta;
            entity.AVKT_TEL_NO = model.AvktTelNo;
            entity.HKK_BURO_AD = model.HkkBuroAd;
            entity.HKK_BURO_ADRES = model.HkkBuroAdres;
            entity.OFIS_TEL_NO = model.OfisTelNo;
        }
        public static Avukat ToEntity(this AvukatModel model) // Model -> Entity Create için
        {
            var entity = new Avukat { AVUKAT_ID = Guid.NewGuid() };
            model.ApplyTo(entity);
            return entity;
        }
    }
}