using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class IcraManager : IIcraService
    {
        private readonly IIcraDal _icraDal;

        public IcraManager(IIcraDal icraDal)
        {
            _icraDal = icraDal;
        }

        public List<IcraDto> GetIcraWithRelations() => _icraDal.GetIcraWithRelations();
        public IcraDto GetByIdWithRelations(Guid id) => _icraDal.GetByIdWithRelations(id);
        public List<IhtarUrunDto> GetIhtarUrun() => _icraDal.GetIhtarUrun();
        public Icra GetById(Guid id) => _icraDal.Get(i => i.ICRA_ID == id);
        public void Add(Icra icra) => _icraDal.Add(icra);
        public void Update(Icra icra) => _icraDal.Update(icra);
    }
}