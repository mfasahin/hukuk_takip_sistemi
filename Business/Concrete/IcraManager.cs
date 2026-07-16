using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System.Collections.Generic;

namespace Business.Concrete
{
    public  class IcraManager : IIcraService
    {
        private readonly IIcraDal _icraDal;

        public IcraManager(IIcraDal icraDal)
        {
            _icraDal = icraDal;
        }

        public List<Icra> GetAll()
        {
            return _icraDal.GetAll();
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

        public Icra GetById(int id)
        {
            return _icraDal.Get(Icra => Icra.ICRA_ID == id);
        }
    }
}