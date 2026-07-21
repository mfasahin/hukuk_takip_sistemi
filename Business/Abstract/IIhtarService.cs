using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IIhtarService
    {
        List<Ihtar> GetAll();
        void Add(Ihtar ihtar);
        void Update(Ihtar ihtar);
        void Delete(Ihtar ihtar);
        Ihtar GetById(Guid id);

        List<IhtarDto> GetIhtarWithRelations();
        IhtarDto GetByIdWithRelations(Guid id);
        Ihtar GetEntityWithUrunlerIncluded(Guid id);
    }
}