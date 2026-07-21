using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class IhtarManager : IIhtarService
    {
        private readonly IIhtarDal _ihtarDal;

        public IhtarManager(IIhtarDal ihtarDal)
        {
            _ihtarDal = ihtarDal;
        }

        public List<Ihtar> GetAll()
        {
            return _ihtarDal.GetAll();
        }

        public void Add(Ihtar ihtar)
        {
            _ihtarDal.Add(ihtar);
        }

        public void Update(Ihtar ihtar)
        {
            _ihtarDal.Update(ihtar);
        }

        public void Delete(Ihtar ihtar)
        {
            _ihtarDal.Delete(ihtar);
        }

        public Ihtar GetById(Guid id)
        {
            return _ihtarDal.Get(Ihtar => Ihtar.IHTAR_ID == id);
        }

        public List<IhtarDto> GetIhtarWithRelations()
        {
            return _ihtarDal.GetIhtarWithRelations();
        }
        public IhtarDto GetByIdWithRelations(Guid id)
        {
            return _ihtarDal.GetByIdWithRelations(id);
        }
        public Ihtar GetEntityWithUrunlerIncluded(Guid id)
        {
            return _ihtarDal.GetEntityWithUrunlerIncluded(id);
        }

        public void UpdateIhtarWithUrunler(IhtarDto model)
        {
            _ihtarDal.UpdateIhtarWithUrunler(model);
        }  
    }
}
