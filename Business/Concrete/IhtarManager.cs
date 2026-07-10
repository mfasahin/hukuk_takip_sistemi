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
    public class IhtarManager : IhtarService
    {
        private IhtarDal _ıhtarDal;

        public IhtarManager(IhtarDal ıhtarDal)
        {
            _ıhtarDal = ıhtarDal;
        }

        public List<Ihtar> GetAll()
        { 
            return _ıhtarDal.GetAll();
        }

        public void Add(Ihtar ıhtar)
        {
            _ıhtarDal.Add(ıhtar);
        }

        public void Update(Ihtar ıhtar)
        {
            _ıhtarDal.Update(ıhtar);
        }

        public void Delete(Ihtar ıhtar)
        {
            _ıhtarDal.Delete(ıhtar);
        }

        public Ihtar GetById(int id)
        {
            return _ıhtarDal.Get(Ihtar => Ihtar.IHTAR_ID  == id);
        }

    }
}
