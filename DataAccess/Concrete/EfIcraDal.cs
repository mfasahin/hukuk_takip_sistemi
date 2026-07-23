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
                       MusteriAd = musteri.MUST_AD + " " + musteri.MUST_SOYAD,

                       IhtarUrunId = ic.IHTAR_URUN_ID,
                       MahkemeId = ic.MAHKEME_ID,
                       MahkemeAd = mahkeme.MAHKEME_AD,
                       IcraDosyaNo = ic.ICRA_DOSYA_NO,
                       IcraTakipTar = ic.ICRA_TAKIP_TAR,

                       UrunId = urun.URUN_ID,
                       UrunAd = urun.URUN_AD,

                       IhtarTarih = ihtar.IHTAR_TAR_ZMN,
                       BorcTutar = ihtar.BORC_TUTAR,

                       AvukatAd = avukat.AVKT_AD,

                       SilTarZmn = ic.SIL_TAR_ZMN
                   };
        }

        public List<IcraDto> GetIcraDto()
        {
            using (var context = new AppDbContext())
            {
                return BuildIcraDtoQuery(context)
                    .Where(dto => dto.SilTarZmn == null)
                    .ToList();
            }
        }

        public IcraDto GetByIdIcra(Guid id)
        {
            using (var context = new AppDbContext())
            {
                return BuildIcraDtoQuery(context)
                    .FirstOrDefault(x => x.IcraId == id);
            }
        }

        // 1. Kademe: Seçilen müşteriye ait, tekrarsız ürün listesi
        public List<UrunDto> GetUrunlerByMusteri(Guid musteriId)
        {
            using (var context = new AppDbContext())
            {
                var raw = (from iu in context.IHTAR_URUN
                           join ihtar in context.IHTAR on iu.IHTAR_ID equals ihtar.IHTAR_ID
                           join urun in context.URUN on iu.URUN_ID equals urun.URUN_ID
                           where ihtar.SIL_TAR_ZMN == null && ihtar.MUSTERI_ID == musteriId
                           select new { urun.URUN_ID, urun.URUN_AD })
                          .Distinct()
                          .ToList();

                return raw.Select(x => new UrunDto
                {
                    UrunId = x.URUN_ID,
                    UrunAd = x.URUN_AD
                }).ToList();
            }
        }

        // 2. Kademe: Seçilen müşteri + ürün kombinasyonuna ait ihtar kayıtları (IhtarUrunId hedefi)
        public List<IhtarUrunDto> GetIhtarlarByMusteriVeUrun(Guid musteriId, Guid urunId)
        {
            using (var context = new AppDbContext())
            {
                var raw = (from iu in context.IHTAR_URUN
                           join ihtar in context.IHTAR on iu.IHTAR_ID equals ihtar.IHTAR_ID
                           where ihtar.SIL_TAR_ZMN == null
                                 && ihtar.MUSTERI_ID == musteriId
                                 && iu.URUN_ID == urunId
                           select new
                           {
                               iu.IHTAR_URUN_ID,
                               iu.IHTAR_ID,
                               iu.URUN_ID,
                               ihtar.IHTAR_TAR_ZMN,
                               ihtar.BORC_TUTAR
                           })
                          .ToList();

                return raw.Select(x => new IhtarUrunDto
                {
                    IhtarUrunId = x.IHTAR_URUN_ID,
                    IhtarId = x.IHTAR_ID,
                    UrunId = x.URUN_ID,
                    IhtarTarih = x.IHTAR_TAR_ZMN,
                    BorcTutar = x.BORC_TUTAR
                }).ToList();
            }
        }
    }
}