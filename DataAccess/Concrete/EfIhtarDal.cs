using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;

namespace DataAccess.Concrete
{
    public class EfIhtarDal : EfEntityRepositoryBase<Ihtar, AppDbContext>, IhtarDal
    {
        // Ihtar'a özel sorgular gerekiyorsa buraya eklenir
    }
}