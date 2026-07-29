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
            return _musteriDal.GetAll(x => x.SIL_TAR_ZMN == null);
        }

        public Musteri GetById(Guid id)
        {
            return _musteriDal.Get(m => m.MUSTERI_ID == id && m.SIL_TAR_ZMN == null);
        }

        // 8 Haneli Benzersiz Müşteri Numarası Üretme
        public string GenerateUniqueMustNo()
        {
            Random random = new Random();
            string newMustNo;
            bool exists;

            do
            {
                // 10000000 ile 99999999 arasında 8 haneli rastgele sayı üretir
                newMustNo = random.Next(10000000, 99999999).ToString();

                // Veritabanında aktif kayıtlar arasında bu numara var mı?
                exists = _musteriDal.Get(x => x.MUST_NO == newMustNo && x.SIL_TAR_ZMN == null) != null;

            } while (exists);

            return newMustNo;
        }

        public void Add(Musteri musteri)
        {
            // 1. FluentValidation Kontrolü
            ValidationResult result = _validator.Validate(musteri);

            if (!result.IsValid)
            {
                string errorMessages = string.Join("\n", result.Errors);
                throw new Exception($"Doğrulama Hataları:\n{errorMessages}");
            }

            // 2. TC Kimlik No Doluysa Benzersizlik Kontrolü
            if (!string.IsNullOrWhiteSpace(musteri.MUST_KIMLIK_NO))
            {
                var existingTc = _musteriDal.Get(m => m.MUST_KIMLIK_NO == musteri.MUST_KIMLIK_NO && m.SIL_TAR_ZMN == null);
                if (existingTc != null)
                {
                    throw new Exception("Bu TC Kimlik No ile kayıtlı aktif bir müşteri zaten mevcut.");
                }
            }

            // 3. Vergi Kimlik No Doluysa Benzersizlik Kontrolü
            if (!string.IsNullOrWhiteSpace(musteri.MUST_VKN_NO))
            {
                var existingVkn = _musteriDal.Get(m => m.MUST_VKN_NO == musteri.MUST_VKN_NO && m.SIL_TAR_ZMN == null);
                if (existingVkn != null)
                {
                    throw new Exception("Bu Vergi Kimlik No ile kayıtlı aktif bir müşteri zaten mevcut.");
                }
            }

            _musteriDal.Add(musteri);
        }

        public void Update(Musteri musteri)
        {
            ValidationResult result = _validator.Validate(musteri);

            if (!result.IsValid)
            {
                string errorMessages = string.Join("\n", result.Errors);
                throw new Exception($"Doğrulama Hataları:\n{errorMessages}");
            }

            _musteriDal.Update(musteri);
        }

        public void Delete(Musteri musteri)
        {
            _musteriDal.Delete(musteri);
        }
    }
}