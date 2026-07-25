using Business.Abstract;
//using Business.Validation;
using DataAccess.Abstract;
using Entity.Concrete;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class MusteriManager : IMusteriService
    {
        private readonly IMusteriDal _musteriDal;
        //private readonly MusteriValidator _validator = new MusteriValidator();

        public MusteriManager(IMusteriDal musteriDal)
        {
            _musteriDal = musteriDal;
        }
        //public ValidationResult Validate(Musteri musteri)
        //{
        //    return _validator.Validate(musteri);
        //}

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

        public Musteri GetById(Guid id)
        {
            return _musteriDal.Get(m => m.MUSTERI_ID == id);
        }
    }
}
