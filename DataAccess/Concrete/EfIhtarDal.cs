using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


public class EfIhtarDal : EfEntityRepositoryBase<Ihtar, AppDbContext>, IIhtarDal
{
    public List<Ihtar> GetIhtarWithRelations()
    {
        using (var context = new AppDbContext())
        {
            var result = context.IHTAR
                .Where(i => i.SIL_TAR_ZMN == null)
                .Include(i => i.Musteri)
                .Include(i => i.Avukat)
                .Include(i => i.Sube)
                .Include(i => i.IhtarUrunler.Select(u => u.Urun)) // iç içe (nested) include: IhtarUrunler koleksiyonu + her birinin Urun'u
                .ToList();

            return result;
        }
    }

}