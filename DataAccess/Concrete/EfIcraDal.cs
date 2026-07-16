using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;

namespace DataAccess.Concrete
{
    public class EfIcraDal : EfEntityRepositoryBase<Icra, AppDbContext>, IIcraDal
    {
        // Icra'ya özel sorgular gerekiyorsa buraya eklenir
    }
}