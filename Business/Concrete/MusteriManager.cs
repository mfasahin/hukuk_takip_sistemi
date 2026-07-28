using Business.Abstract;
using Business.Validation;
using DataAccess.Abstract;
using Entity.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class MusteriManager : IMusteriService
    {
        private readonly IMusteriDal _musteriDal;
        private readonly MusteriValidator _validator = new MusteriValidator();

        public MusteriManager(IMusteriDal musteriDal)
        {
            _musteriDal = musteriDal;
        }
        public ValidationResult Validate(Musteri musteri)
        {
            return _validator.Validate(musteri);
        }

        public List<Musteri> GetAll()
        {
            return _musteriDal.GetAll();
        }

        public void Add(Musteri musteri)
        {
            
            MusteriValidator validator = new MusteriValidator();
            ValidationResult result = validator.Validate(musteri);

            if (!result.IsValid)
            {
                // Hataları fırlatabilir veya bir liste şeklinde dönebilirsin
                string errorMessages = string.Join("\n", result.Errors);
                throw new Exception($"Doğrulama Hataları:\n{errorMessages}");
            }

            // TC Kimlik No zaten var mı?
            var existing = _musteriDal.Get(m => m.MUST_KIMLIK_NO == musteri.MUST_KIMLIK_NO);
            if (existing != null)
            {
                throw new Exception("Bu TC Kimlik No ile kayıtlı müşteri zaten mevcut.");
            }
            //Doğrulama başarılıysa veritabanı kayıt kodları çalışır

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
