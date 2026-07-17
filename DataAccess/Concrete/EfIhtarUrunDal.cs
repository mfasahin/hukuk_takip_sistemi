using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;

namespace DataAccess.Concrete
{
    public class EfIhtarUrunDal : EfEntityRepositoryBase<IhtarUrun, AppDbContext>, IIhtarUrunDal
    {
        // IhtarUrun'e özel sorgular gerekiyorsa buraya eklenir
    }
}
