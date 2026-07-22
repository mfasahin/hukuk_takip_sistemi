using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete
{
    public class EfIcraDal : EfEntityRepositoryBase<Icra, AppDbContext>, IIcraDal
    {
        // Ortak DTO sorgusu - hem liste hem tekil kayıt için kullanılır.
        private IQueryable<IcraDto> BuildIcraDtoQuery(AppDbContext context)
        {
            return from ic in context.ICRA
                   join iu in context.IHTAR_URUN on ic.IHTAR_URUN_ID equals iu.IHTAR_URUN_ID
                   join ihtar in context.IHTAR on iu.IHTAR_ID equals ihtar.IHTAR_ID
                   join urun in context.URUN on iu.URUN_ID equals urun.URUN_ID
                   join musteri in context.MUSTERI on ihtar.MUSTERI_ID equals musteri.MUSTERI_ID
                   join avukat in context.AVUKAT on ihtar.AVUKAT_ID equals avukat.AVUKAT_ID
                   join mahkeme in context.ICRA_MAHKEME on ic.MAHKEME_ID equals mahkeme.MAHKEME_ID
                   select new IcraDto
                   {
                       IcraId = ic.ICRA_ID,
                       MusteriId = musteri.MUSTERI_ID,
                       IhtarUrunId = ic.IHTAR_URUN_ID,
                       MahkemeId = ic.MAHKEME_ID,
                       MahkemeAd = mahkeme.MAHKEME_AD,
                       IcraDosyaNo = ic.ICRA_DOSYA_NO,
                       IcraTakipTar = ic.ICRA_TAKIP_TAR,     
                       UrunAd = urun.URUN_AD,
                       IhtarTarih = ihtar.IHTAR_TAR_ZMN,
                       BorcTutar = ihtar.BORC_TUTAR,

                       MusteriAd = musteri.MUST_AD + " " + musteri.MUST_SOYAD,
                       AvukatAd = avukat.AVKT_AD,

                       SilTarZmn = ic.SIL_TAR_ZMN
                   };
        }

        public List<IcraDto> GetIcraWithRelations()
        {
            using (var context = new AppDbContext())
            {
                return BuildIcraDtoQuery(context)
                    .Where(dto => dto.SilTarZmn == null)
                    .ToList();
            }
        }

        public IcraDto GetByIdWithRelations(Guid id)
        {
            using (var context = new AppDbContext())
            {
                return BuildIcraDtoQuery(context)
                    .FirstOrDefault(x => x.IcraId == id);
            }
        }

        // Create/Update formundaki "hangi ihtar-ürün'e icra bağlanacak" dropdown'ı için
        public List<IhtarUrunDto> GetIhtarUrun()
        {
            using (var context = new AppDbContext())
            {
                // 1. Adım: Ham veriyi SQL'e çevrilebilir haliyle çek
                var raw = (from iu in context.IHTAR_URUN
                           join ihtar in context.IHTAR on iu.IHTAR_ID equals ihtar.IHTAR_ID
                           join urun in context.URUN on iu.URUN_ID equals urun.URUN_ID
                           join musteri in context.MUSTERI on ihtar.MUSTERI_ID equals musteri.MUSTERI_ID
                           where ihtar.SIL_TAR_ZMN == null
                           select new
                           {
                               iu.IHTAR_URUN_ID,
                               iu.IHTAR_ID,
                               iu.URUN_ID,
                               MusteriAdi = musteri.MUST_AD + " " + musteri.MUST_SOYAD,
                               UrunAdi = urun.URUN_AD,
                               ihtar.IHTAR_TAR_ZMN
                           })
                          .ToList();

                // 2. Adım: DTO'ya dönüştür (formatlama gerekiyorsa burada, bellekte yapılır)
                var result = raw.Select(x => new IhtarUrunDto
                {
                    IhtarUrunId = x.IHTAR_URUN_ID,
                    IhtarId = x.IHTAR_ID,
                    UrunId = x.URUN_ID,
                    UrunAd = x.UrunAdi,
                    MusteriAd = x.MusteriAdi,
                    IhtarTarih = x.IHTAR_TAR_ZMN
                }).ToList();

                return result;
            }
        }

        public List<IhtarUrunDto> GetIhtarUrunByMusteri(Guid musteriId)
        {
            using (var context = new AppDbContext())
            {
                var raw = (from iu in context.IHTAR_URUN
                           join ihtar in context.IHTAR on iu.IHTAR_ID equals ihtar.IHTAR_ID
                           join urun in context.URUN on iu.URUN_ID equals urun.URUN_ID
                           where ihtar.SIL_TAR_ZMN == null && ihtar.MUSTERI_ID == musteriId
                           select new
                           {
                               iu.IHTAR_URUN_ID,
                               iu.IHTAR_ID,
                               iu.URUN_ID,
                               UrunAdi = urun.URUN_AD,
                               ihtar.IHTAR_TAR_ZMN
                           })
                          .ToList();

                return raw.Select(x => new IhtarUrunDto
                {
                    IhtarUrunId = x.IHTAR_URUN_ID,
                    IhtarId = x.IHTAR_ID,
                    UrunId = x.URUN_ID,
                    UrunAd = x.UrunAdi,
                    IhtarTarih = x.IHTAR_TAR_ZMN
                }).ToList();
            }
        }
    }
}