using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;
using System.Linq;

namespace DataAccess.Concrete
{
    public class EfKullaniciDal : EfEntityRepositoryBase<Kullanici, AppDbContext>, IKullaniciDal
    {
        public Kullanici GetByUsername(string kullaniciAd)
        {
            using (var context = new AppDbContext())
            {
                return context.KULLANICI
                    .FirstOrDefault(k => k.KULLANICI_AD == kullaniciAd && k.SIL_TAR_ZMN == null);
            }
        }
    }
}