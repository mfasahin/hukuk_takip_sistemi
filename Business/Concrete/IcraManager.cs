using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public  class IcraManager : IcraService
    {
        private IcraDal _icraDal;

        public IcraManager(IcraDal icraDal)
        {
            _icraDal = icraDal;
        }

        public List<Icra> GetAll()
        {
            return _icraDal.GetAll();
        }

        public void Add(Icra ıcra)
        {
            _icraDal.Add(ıcra);
        }

        public void Update(Icra ıcra)
        {
            _icraDal.Update(ıcra);
        }

        public void Delete(Icra ıcra)
        {
            _icraDal.Delete(ıcra);
        }

        public Icra GetById(int id)
        {
            return _icraDal.Get(Icra => Icra.ICRA_ID == id);
        }
    }
}
