using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class MusteriManager : IMusteriService
    {
        private IMusteriDal _musteriDal;

        public MusteriManager(IMusteriDal musteriDal)
        {
            _musteriDal = musteriDal;
        }

        public List<Musteri> GetAll()
        {
            return _musteriDal.GetAll();
        }

        public void Add(Musteri musteri)
        {
            _musteriDal.Add(musteri);
        }

        public void Update(Musteri musteri)
        {
            _musteriDal.Update(musteri);
        }

        public void Delete(Musteri musteri)
        {
            _musteriDal.Delete(musteri);
        }

        public Musteri GetById(int id)
        {
            return _musteriDal.Get(m => m.MUSTERI_ID == id);
        }
    }
}
