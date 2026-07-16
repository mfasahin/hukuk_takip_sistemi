using Core.DataAccess;
using Entity.Concrete;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IIhtarDal : IEntityRepository<Ihtar>
    {
        List<Ihtar> GetIhtarWithRelations();
    }
}
