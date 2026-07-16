using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using System.Collections.Generic;
using System.Linq;

public class EfIhtarDal : EfEntityRepositoryBase<Ihtar, AppDbContext>, IIhtarDal
{
    public List<Ihtar> GetIhtarWithRelations()
    {
        using (var context = new AppDbContext())
        {
            var result = from i in context.IHTAR
                         join m in context.MUSTERI on i.MUSTERI_ID equals m.MUSTERI_ID
                         join a in context.AVUKAT on i.AVUKAT_ID equals a.AVUKAT_ID
                         join s in context.SUBE on i.SUBE_ID equals s.SUBE_ID
                         where i.SIL_TAR_ZMN == null
                         select i; // sadece entity döndür


            return result.ToList();
        }
    }
}