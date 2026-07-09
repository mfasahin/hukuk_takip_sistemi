using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;

namespace DataAccess.Concrete
{
    public class EfSubeDal : EfEntityRepositoryBase<Sube, AppDbContext>, ISubeDal
    {
        // Sube’ye özel sorgular gerekiyorsa buraya eklenir
    }
}
