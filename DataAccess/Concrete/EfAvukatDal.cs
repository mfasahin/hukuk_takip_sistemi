using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;

namespace DataAccess.Concrete
{
    public class EfAvukatDal : EfEntityRepositoryBase<Avukat, AppDbContext>, IAvukatDal
    {
        // Avukat'a özel sorgular gerekiyorsa buraya eklenir
    }
}