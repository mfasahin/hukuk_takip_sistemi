using Core.DataAccess;
using Entity.Concrete;

namespace DataAccess.Abstract
{
    public interface IKullaniciDal : IEntityRepository<Kullanici>
    {
        Kullanici GetByUsername(string kullaniciAd);
    }
}