using Core.DataAccess;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IIhtarDal : IEntityRepository<Ihtar>
    {
        List<IhtarDto> GetIhtarDto();
        IhtarDto GetByIdIhtarDto(Guid id);
        Ihtar GetIhtarWithUrunler(Guid id);
        void UpdateIhtarWithUrunler(IhtarDto model);

    }
}
