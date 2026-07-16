using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;

namespace DataAccess.Concrete
{
    public class EfUrunDal : EfEntityRepositoryBase<Urun, AppDbContext>, IUrunDal
    {
        // Urun'e özel sorgular gerekiyorsa buraya eklenir
    }
}