using Core.DataAccess;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IIhtarDal : IEntityRepository<Ihtar>
    {
        List<IhtarDto> GetIhtarWithRelations();
        Ihtar GetByIdWithRelations(Guid id);
    }
}
