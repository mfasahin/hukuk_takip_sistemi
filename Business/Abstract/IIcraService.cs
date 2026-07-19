using Entity.Concrete;
using Entity.Dto;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IIcraService
    {
        List<IcraDto> GetIcraWithRelations();
        IcraDto GetByIdWithRelations(int id);
        List<IhtarUrunDto> GetIhtarUrun();
        Icra GetById(int id);
        void Add(Icra icra);
        void Update(Icra icra);
    }
}