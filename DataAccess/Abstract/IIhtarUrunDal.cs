using Core.DataAccess;
using Entity.Concrete;
using System;

namespace DataAccess.Abstract
{
    public interface IIhtarUrunDal : IEntityRepository<IhtarUrun>
    {
        void Delete(Guid id);
    }
}