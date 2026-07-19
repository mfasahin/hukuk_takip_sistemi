using Core.DataAccess;
using Entity.Concrete;
using Entity.Dto;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IIcraDal : IEntityRepository<Icra>
    {
        List<IcraDto> GetIcraWithRelations();
        IcraDto GetByIdWithRelations(int id);
        List<IhtarUrunDto> GetIhtarUrun();
    }
}