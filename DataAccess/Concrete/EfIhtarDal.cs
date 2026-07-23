using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete
{
    public class EfIhtarDal : EfEntityRepositoryBase<Ihtar, AppDbContext>, IIhtarDal
    {
        // Ortak DTO sorgusu - hem liste hem tekil kayıt için kullanılır.
        private IQueryable<IhtarDto> IhtarDtoQuery(AppDbContext context)
        {
            return from ihtar in context.IHTAR
                   join musteri in context.MUSTERI on ihtar.MUSTERI_ID equals musteri.MUSTERI_ID
                   join avukat in context.AVUKAT on ihtar.AVUKAT_ID equals avukat.AVUKAT_ID
                   join sube in context.SUBE on ihtar.SUBE_ID equals sube.SUBE_ID
                   join ihtarurun in context.IHTAR_URUN on ihtar.IHTAR_ID equals ihtarurun.IHTAR_ID
                   join urun in context.URUN on ihtarurun.URUN_ID equals urun.URUN_ID
                   select new IhtarDto
                   {
                       IhtarId = ihtar.IHTAR_ID,
                       BorcTutar = ihtar.BORC_TUTAR,
                       IhtarTarih = ihtar.IHTAR_TAR_ZMN,

                       MusteriId = musteri.MUSTERI_ID,
                       MusteriNo = musteri.MUST_NO,
                       MusteriAd = musteri.MUST_AD,
                       MusteriSoyad = musteri.MUST_SOYAD,

                       SubeId = sube.SUBE_ID,
                       SubeAd = sube.SUBE_ADI,
                       SubeKod = sube.SUBE_KODU,

                       AvukatId = avukat.AVUKAT_ID,
                       AvukatAd = avukat.AVKT_AD,
                       AvukatSoyad = avukat.AVKT_SOYAD,

                       SilTarZmn = ihtar.SIL_TAR_ZMN,

                       UrunAd = urun.URUN_AD
                   };
        }


        // silinmemiş ihtar bilgileri DTO listesi olarak döner
        public List<IhtarDto> GetIhtarDto()
        {
            using (var context = new AppDbContext())
            {
                return IhtarDtoQuery(context)
                    .Where(dto => dto.SilTarZmn == null)
                    .ToList();
            }
        }

        public IhtarDto GetByIdIhtarDto(Guid id)
        {
            using (var context = new AppDbContext())
            {
                var dto = IhtarDtoQuery(context)
                    .FirstOrDefault(x => x.IhtarId == id);

                if (dto != null)
                {
                    // İhtara bağlı ürün ilişkisini bul
                    var ihtarUrun = context.IHTAR_URUN
                        .FirstOrDefault(iu => iu.IHTAR_ID == id);

                    if (ihtarUrun != null)
                    {
                        dto.UrunId = ihtarUrun.URUN_ID;
                    }
                }

                return dto;
            }
        }

    }

}