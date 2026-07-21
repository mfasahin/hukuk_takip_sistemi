//using Core.DataAccess.EntityFramework;
//using DataAccess.Abstract;
//using Entity.Concrete;
//using Entity.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace DataAccess.Concrete
//{
//    public class EfIcraDal : EfEntityRepositoryBase<Icra, AppDbContext>, IIcraDal
//    {
//        public List<IcraDto> GetIcraWithRelations()
//        {
//            using (var context = new AppDbContext())
//            {
//                var result = from ic in context.ICRA
//                             join iu in context.IHTAR_URUN on ic.IHTAR_URUN_ID equals iu.IHTAR_URUN_ID
//                             join ihtar in context.IHTAR on iu.IHTAR_ID equals ihtar.IHTAR_ID
//                             join urun in context.URUN on iu.URUN_ID equals urun.URUN_ID
//                             join musteri in context.MUSTERI on ihtar.MUSTERI_ID equals musteri.MUSTERI_ID
//                             join avukat in context.AVUKAT on ihtar.AVUKAT_ID equals avukat.AVUKAT_ID
//                             join sube in context.SUBE on ihtar.SUBE_ID equals sube.SUBE_ID
//                             join mahkeme in context.ICRA_MAHKEME on ic.MAHKEME_ID equals mahkeme.MAHKEME_ID
//                             where ic.SIL_TAR_ZMN == null
//                             select new IcraDto
//                             {
//                                 IcraId = ic.ICRA_ID,               // Guid
//                                 IhtarUrunId = ic.IHTAR_URUN_ID,   // Guid
//                                 MahkemeId = ic.MAHKEME_ID,        // Guid
//                                 MahkemeAd = mahkeme.MAHKEME_AD,
//                                 IcraTakipTar = ic.ICRA_TAKIP_TAR,
//                                 IcraDosyaNo = ic.ICRA_DOSYA_NO,

//                                 UrunAd = urun.URUN_AD,
//                                 IhtarTarih = ihtar.IHTAR_TAR_ZMN,
//                                 BorcTutar = ihtar.BORC_TUTAR,

//                                 MusteriAd = musteri.MUST_AD + " " + musteri.MUST_SOYAD,
//                                 AvukatAd = avukat.AVKT_AD,
//                                 SubeAd = sube.SUBE_ADI,

//                                 SilTarZmn = ic.SIL_TAR_ZMN
//                             };

//                return result.ToList();
//            }
//        }

//        public IcraDto GetByIdWithRelations(Guid id)
//        {
//            using (var context = new AppDbContext())
//            {
//                var result = from ic in context.ICRA
//                             join iu in context.IHTAR_URUN on ic.IHTAR_URUN_ID equals iu.IHTAR_URUN_ID
//                             join ihtar in context.IHTAR on iu.IHTAR_ID equals ihtar.IHTAR_ID
//                             join urun in context.URUN on iu.URUN_ID equals urun.URUN_ID
//                             join musteri in context.MUSTERI on ihtar.MUSTERI_ID equals musteri.MUSTERI_ID
//                             join avukat in context.AVUKAT on ihtar.AVUKAT_ID equals avukat.AVUKAT_ID
//                             join sube in context.SUBE on ihtar.SUBE_ID equals sube.SUBE_ID
//                             join mahkeme in context.ICRA_MAHKEME on ic.MAHKEME_ID equals mahkeme.MAHKEME_ID
//                             where ic.ICRA_ID == id   // Guid karşılaştırması
//                             select new IcraDto
//                             {
//                                 IcraId = ic.ICRA_ID,
//                                 IhtarUrunId = ic.IHTAR_URUN_ID,
//                                 MahkemeId = ic.MAHKEME_ID,
//                                 MahkemeAd = mahkeme.MAHKEME_AD,
//                                 IcraTakipTar = ic.ICRA_TAKIP_TAR,
//                                 IcraDosyaNo = ic.ICRA_DOSYA_NO,

//                                 UrunAd = urun.URUN_AD,
//                                 IhtarTarih = ihtar.IHTAR_TAR_ZMN,
//                                 BorcTutar = ihtar.BORC_TUTAR,

//                                 MusteriAd = musteri.MUST_AD + " " + musteri.MUST_SOYAD,
//                                 AvukatAd = avukat.AVKT_AD,
//                                 SubeAd = sube.SUBE_ADI,

//                                 SilTarZmn = ic.SIL_TAR_ZMN
//                             };

//                return result.FirstOrDefault();
//            }
//        }

//        public List<IhtarUrunDto> GetIhtarUrun()
//        {
//            using (var context = new AppDbContext())
//            {
//                var raw = (from iu in context.IHTAR_URUN
//                           join ihtar in context.IHTAR on iu.IHTAR_ID equals ihtar.IHTAR_ID
//                           join urun in context.URUN on iu.URUN_ID equals urun.URUN_ID
//                           join musteri in context.MUSTERI on ihtar.MUSTERI_ID equals musteri.MUSTERI_ID
//                           where ihtar.SIL_TAR_ZMN == null
//                           select new
//                           {
//                               iu.IHTAR_URUN_ID,   // Guid
//                               MusteriAdi = musteri.MUST_AD + " " + musteri.MUST_SOYAD,
//                               UrunAdi = urun.URUN_AD,
//                               ihtar.IHTAR_TAR_ZMN
//                           })
//                          .ToList();

//                var result = raw.Select(x => new IhtarUrunDto
//                {
//                    IhtarUrunId = x.IHTAR_URUN_ID,
//                    DisplayText = x.MusteriAdi + " - " + x.UrunAdi + " - " + x.IHTAR_TAR_ZMN.ToString("dd.MM.yyyy")
//                }).ToList();

//                return result;
//            }
//        }
//    }
//}
