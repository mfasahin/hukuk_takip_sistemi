using Business.Abstract;
using Business.Validation;
using DataAccess.Abstract;
using Entity.Concrete;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class AvukatManager : IAvukatService
    {
        private readonly IAvukatDal _avukatDal;
        private readonly AvukatValidator _validator = new AvukatValidator();

        public AvukatManager(IAvukatDal avukatDal)
        {
            _avukatDal = avukatDal;
        }
        public ValidationResult Validate(Avukat avukat)
        {
            return _validator.Validate(avukat);
        }
        public List<Avukat> GetAll()
        {
            return _avukatDal.GetAll();
        }

        public void Add(Avukat avukat)
        {
            // 1. FluentValidation Çalıştırma
            AvukatValidator validator = new AvukatValidator();
            ValidationResult result = validator.Validate(avukat);

            if (!result.IsValid)
            {
                string errorMessages = string.Join("\n", result.Errors);
                throw new Exception($"Doğrulama Hataları:\n{errorMessages}");
            }

            // 2. TBB Sicil No Mükerrer Kontrolü (Silinmemiş olanlar arasında)
            var existing = _avukatDal.Get(a => a.TBB_SICIL_NO == avukat.TBB_SICIL_NO && a.SIL_TAR_ZMN == null);
            if (existing != null)
            {
                throw new Exception("Bu TBB Sicil Numarası ile kayıtlı bir avukat zaten mevcut.");
            }

            // 3. Veritabanına Ekleme
            _avukatDal.Add(avukat);
        }

        public void Update(Avukat avukat)
        {
            // 1. FluentValidation Çalıştırma
            AvukatValidator validator = new AvukatValidator();
            ValidationResult result = validator.Validate(avukat);

            if (!result.IsValid)
            {
                string errorMessages = string.Join("\n", result.Errors);
                throw new Exception($"Doğrulama Hataları:\n{errorMessages}");
            }

            // 2. Güncelleme
            _avukatDal.Update(avukat);
        }

        public void Delete(Avukat avukat)
        {
            
            _avukatDal.Delete(avukat);
        }

        public Avukat GetById(Guid id)
        {
            return _avukatDal.Get(a => a.AVUKAT_ID == id && a.SIL_TAR_ZMN == null);
        }
    }
}