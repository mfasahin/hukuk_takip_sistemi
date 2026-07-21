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

        public List<IcraDto> GetIcraWithRelations()
        {
            return _icraDal.GetIcraWithRelations();
        }

        public IcraDto GetByIdWithRelations(Guid id)
        {
            return _icraDal.GetByIdWithRelations(id);
        }

        public List<IhtarUrunDto> GetIhtarUrun()
        {
            return _icraDal.GetIhtarUrun();
        }

        public Icra GetById(Guid id)
        {
            return _icraDal.Get(i => i.ICRA_ID == id);
        }

        public void Add(Icra icra)
        {
            _icraDal.Add(icra);
        }

        public void Update(Icra icra)
        {
            _icraDal.Update(icra);
        }

        public void Delete(Icra icra)
        {
            _icraDal.Delete(icra);
        }
    }
}