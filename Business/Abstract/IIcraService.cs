using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IIcraService
    {
        List<IcraDto> GetIcraWithRelations();
        IcraDto GetByIdWithRelations(Guid id);
        List<IhtarUrunDto> GetIhtarUrun();
        Icra GetById(Guid id);
        void Add(Icra icra);
        void Update(Icra icra);
    }
}