using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


public class EfIhtarDal : EfEntityRepositoryBase<Ihtar, AppDbContext>, IIhtarDal
{
    public List<IhtarDto> GetIhtarWithRelations()
    {
        using (var context = new AppDbContext())
        {
            var result = from ihtar in context.IHTAR
                         join musteri in context.MUSTERI on ihtar.MUSTERI_ID equals musteri.MUSTERI_ID
                         join avukat in context.AVUKAT on ihtar.AVUKAT_ID equals avukat.AVUKAT_ID
                         join sube in context.SUBE on ihtar.SUBE_ID equals sube.SUBE_ID
                         select new IhtarDto
                         {
                             IhtarId = ihtar.IHTAR_ID,
                             BorcTutar = ihtar.BORC_TUTAR,
                             //IhtarTarih = ihtar.IHTAR_TAR_ZMN,
                             MusteriAd = musteri.MUST_AD,
                             AvukatAd = avukat.AVKT_AD,
                             SubeAd = sube.SUBE_ADI
                         };

            return result.ToList();
        }
    }
}