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

        public List<MusteriDto> GetIhtariOlanMusteriler()
        {
            using (var context = new AppDbContext())
            {
                return (from musteri in context.MUSTERI
                        join ihtar in context.IHTAR on musteri.MUSTERI_ID equals ihtar.MUSTERI_ID
                        where musteri.SIL_TAR_ZMN == null && ihtar.SIL_TAR_ZMN == null
                        select new MusteriDto
                        {
                            MusteriId = musteri.MUSTERI_ID,
                            MustAd = musteri.MUST_AD,
                            MustSoyad = musteri.MUST_SOYAD
                        })
                        .Distinct()
                        .ToList();
            }
        }

        // isForUpdate = false eklendi
        // 1. Kademe: Seçilen Müşteriye ait İhtar çekilmiş Ürünlerin getirilmesi
        public List<UrunDto> GetUrunlerByMusteri(Guid musteriId, bool isForUpdate = false)
        {
            using (var context = new AppDbContext())
            {
                var query = from iu in context.IHTAR_URUN
                            join ihtar in context.IHTAR on iu.IHTAR_ID equals ihtar.IHTAR_ID
                            join urun in context.URUN on iu.URUN_ID equals urun.URUN_ID
                            where ihtar.MUSTERI_ID == musteriId
                                  && ihtar.SIL_TAR_ZMN == null
                                  && urun.SIL_TAR_ZMN == null
                            select new { iu, ihtar, urun };

                // CREATE MODALI İÇİN (!isForUpdate):
                // Eğer müşterinin bu ÜRÜNÜNE ait açılmış herhangi bir aktif İCRA kaydı varsa, bu ürünü HİÇ getirme!
                if (!isForUpdate)
                {
                    query = query.Where(x => !(from icra in context.ICRA
                                               join iu2 in context.IHTAR_URUN on icra.IHTAR_URUN_ID equals iu2.IHTAR_URUN_ID
                                               join ihtar2 in context.IHTAR on iu2.IHTAR_ID equals ihtar2.IHTAR_ID
                                               where icra.SIL_TAR_ZMN == null
                                                     && ihtar2.SIL_TAR_ZMN == null
                                                     && ihtar2.MUSTERI_ID == musteriId
                                                     && iu2.URUN_ID == x.urun.URUN_ID // Doğrudan URUN_ID seviyesinde engelleme
                                               select icra).Any());
                }

                var rawList = query.Select(x => new
                {
                    x.urun.URUN_ID,
                    x.urun.URUN_AD
                })
                .Distinct()
                .ToList();

                return rawList.Select(x => new UrunDto
                {
                    UrunId = x.URUN_ID,
                    UrunAd = x.URUN_AD
                }).ToList();
            }
        }

        // 2. Kademe: Müşteri + Ürün seçildiğinde, bunlara ait IHTAR_URUN kayıtlarını ve İhtar detayını getirir
        public List<IhtarUrunDto> GetIhtarlarByMusteriVeUrun(Guid musteriId, Guid urunId)
        {
            using (var context = new AppDbContext())
            {
                var rawList = (from iu in context.IHTAR_URUN
                               join ihtar in context.IHTAR on iu.IHTAR_ID equals ihtar.IHTAR_ID
                               where ihtar.MUSTERI_ID == musteriId
                                     && iu.URUN_ID == urunId
                                     && ihtar.SIL_TAR_ZMN == null
                               select new
                               {
                                   iu.IHTAR_URUN_ID,
                                   iu.IHTAR_ID,
                                   iu.URUN_ID,
                                   ihtar.IHTAR_TAR_ZMN,
                                   ihtar.BORC_TUTAR
                               }).ToList();

                return rawList.Select(x => new IhtarUrunDto
                {
                    IhtarUrunId = x.IHTAR_URUN_ID,
                    IhtarId = x.IHTAR_ID,
                    UrunId = x.URUN_ID,
                    IhtarTarih = x.IHTAR_TAR_ZMN,
                    BorcTutar = x.BORC_TUTAR
                }).ToList();
            }
        }

        public bool IhtaraBagliIcraVarMi(Guid ihtarUrunId)
        {
            using (var context = new AppDbContext())
            {
                return context.ICRA.Any(i => i.IHTAR_URUN_ID == ihtarUrunId && i.SIL_TAR_ZMN == null);
            }
        }
    }
}