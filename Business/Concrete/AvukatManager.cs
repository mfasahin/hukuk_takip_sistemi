using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class AvukatManager : IAvukatService
    {
        private IAvukatDal _avukatDal;

        public AvukatManager(IAvukatDal avukatDal)
        {
            _avukatDal = avukatDal;
        }

        public List<Avukat> GetAll()
        {
            return _avukatDal.GetAll();
        }

        public void Add(Avukat avukat)
        {
            _avukatDal.Add(avukat);
        }

        public void Update(Avukat avukat)
        {
            _avukatDal.Update(avukat);
        }
        public void Delete(Avukat avukat)
        {
            _avukatDal.Delete(avukat);
        }

        public Avukat GetById(int id)
        {
            return _avukatDal.Get(a => a.AVUKAT_ID == id);
        }
    }
}
