using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class UrunManager : IUrunService
    {
        private IUrunDal _urunDal;
        public UrunManager(IUrunDal urunDal)
        {
            _urunDal = urunDal;
        }

        public List<Urun> GetAll()
        {
            return _urunDal.GetAll();
        }

        public void Add(Urun urun)
        {
            _urunDal.Add(urun);
        }

        public void Update(Urun urun)
        {
            _urunDal.Update(urun);
        }
        public void Delete(Urun urun)
        {
            _urunDal.Delete(urun);
        }

        public Urun GetById(int id)
        {
            return _urunDal.Get(u => u.URUN_ID == id);
        }
    }
}
