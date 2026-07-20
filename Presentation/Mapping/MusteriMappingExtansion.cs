using Entity.Concrete;
using Presentation.Models;
using System;

namespace Presentation.Mapping
{
    public static class MusteriMappingExtensions
    {
        // Entity -> Model (listeleme, GET için)
        public static MusteriModel ToModel(this Musteri entity)
        {
            if (entity == null) return null;

            return new MusteriModel
            {
                MusteriId = entity.MUSTERI_ID,
                MustNo = entity.MUST_NO,
                MustAd = entity.MUST_AD,
                MustSoyad = entity.MUST_SOYAD,
                MustKimlikNo = entity.MUST_KIMLIK_NO,
                MustVknNo = entity.MUST_VKN_NO,
                MustEposta = entity.MUST_EPOSTA,
                MustTelNo = entity.MUST_TEL_NO,
                SilTarZmn = entity.SIL_TAR_ZMN
            };
        }

        // Model'deki alanları var olan bir entity'ye uygular (Update için)
        public static void ApplyTo(this MusteriModel model, Musteri entity)
        {
            entity.MUST_NO = model.MustNo;
            entity.MUST_AD = model.MustAd;
            entity.MUST_SOYAD = model.MustSoyad;
            entity.MUST_KIMLIK_NO = model.MustKimlikNo;
            entity.MUST_VKN_NO = model.MustVknNo;
            entity.MUST_EPOSTA = model.MustEposta;
            entity.MUST_TEL_NO = model.MustTelNo;
        }

        // Model -> yeni Entity (Create için)
        public static Musteri ToEntity(this MusteriModel model)
        {
            var entity = new Musteri { MUSTERI_ID = Guid.NewGuid() };
            model.ApplyTo(entity);
            return entity;
        }
    }
}
