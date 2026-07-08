using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfMusteriDal : EfEntityRepositoryBase<Musteri, AppDbContext>, IMusteriDal
    {
        // Musteri’ye özel sorgular gerekiyorsa buraya eklenir
    }
}
