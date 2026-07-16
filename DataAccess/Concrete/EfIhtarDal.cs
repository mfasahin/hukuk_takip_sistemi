using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete
{
    public class EfIhtarDal : EfEntityRepositoryBase<Ihtar, AppDbContext>, IIhtarDal
    {
        //İhtarları müşteri, avukat ve şube ile join ederek getir
        public List<Ihtar> GetIhtarWithRelations()
        {
            using (var context = new AppDbContext())
            {
                var result = from i in context.IHTAR
                             join m in context.MUSTERI on i.MUSTERI_ID equals m.MUSTERI_ID
                             join a in context.AVUKAT on i.AVUKAT_ID equals a.AVUKAT_ID
                             join s in context.SUBE on i.SUBE_ID equals s.SUBE_ID
                             where i.SIL_TAR_ZMN == null
                             select new Ihtar
                             {
                                 IHTAR_ID = i.IHTAR_ID,
                                 MUSTERI_ID = i.MUSTERI_ID,
                                 AVUKAT_ID = i.AVUKAT_ID,
                                 SUBE_ID = i.SUBE_ID,
                                 BORC_TUTAR = i.BORC_TUTAR,
                                 IHTAR_TAR_ZMN = i.IHTAR_TAR_ZMN,
                                 GRS_TAR_ZMN = i.GRS_TAR_ZMN,
                                 GRS_KULLANICI_ID = i.GRS_KULLANICI_ID,
                                 GNC_TAR_ZMN = i.GNC_TAR_ZMN,
                                 GNC_KULLANICI_ID = i.GNC_KULLANICI_ID,
                                 SIL_TAR_ZMN = i.SIL_TAR_ZMN,
                                 SIL_KULLANICI_ID = i.SIL_KULLANICI_ID,

                                 // Navigation objelerini manuel dolduruyoruz
                                 Musteri = m,
                                 Avukat = a,
                                 Sube = s
                             };

                return result.ToList();
            }
        }

    }
}