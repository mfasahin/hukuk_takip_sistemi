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
        public List<IcraDto> GetIcraDto()
        {
            return _icraDal.GetIcraDto();
        }

        public IcraDto GetByIdIcra(Guid id)
        {
            return _icraDal.GetByIdIcra(id);
        }
        public List<UrunDto> GetUrunlerByMusteri(Guid musteriId)
        {
            return _icraDal.GetUrunlerByMusteri(musteriId);
        }
        public List<IhtarUrunDto> GetIhtarlarByMusteriVeUrun(Guid musteriId, Guid urunId)
        {
            return _icraDal.GetIhtarlarByMusteriVeUrun(musteriId, urunId);
        }
    }
}