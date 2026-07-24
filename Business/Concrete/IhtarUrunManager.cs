using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class IhtarUrunManager : IIhtarUrunService
    {
        private readonly IIhtarUrunDal _ihtarUrunDal;
        private readonly IUrunService _urunDal;

        public IhtarUrunManager(IIhtarUrunDal ihtarUrunDal, IUrunService urunDal)
        {
            _ihtarUrunDal = ihtarUrunDal;
            _urunDal = urunDal;
        }

        public List<IhtarUrun> GetAll()
        {
            return _ihtarUrunDal.GetAll();
        }

        public void Add(IhtarUrun ihtarUrun)
        {
            _ihtarUrunDal.Add(ihtarUrun);
        }

        public void Update(IhtarUrun ihtarUrun)
        {
            _ihtarUrunDal.Update(ihtarUrun);
        }
        public void Delete(IhtarUrun ihtarUrun)
        {
            _ihtarUrunDal.Delete(ihtarUrun);
        }
        public void Delete(Guid id)
        {
            _ihtarUrunDal.Delete(id);
        }

        public IhtarUrun GetById(Guid id)
        {
            return _ihtarUrunDal.Get(i => i.IHTAR_URUN_ID == id);
        }

        public List<IhtarUrun> GetByIhtarId(Guid ihtarId)
        {
            return _ihtarUrunDal.GetAll(x => x.IHTAR_ID == ihtarId).ToList();
        }

        public bool UruneBagliIhtarVarMi(Guid urunId)
        {
            return _ihtarUrunDal.UruneBagliIhtarVarMi(urunId);
        }
    }
}
