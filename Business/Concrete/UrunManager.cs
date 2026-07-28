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
    public class UrunManager : IUrunService
    {
        private readonly IUrunDal _urunDal;
        private readonly UrunValidator _validator = new UrunValidator();
        public UrunManager(IUrunDal urunDal)
        {
            _urunDal = urunDal;
        }
        public ValidationResult Validate(Urun urun)
        {
            return _validator.Validate(urun);
        }
        public List<Urun> GetAll()
        {
            return _urunDal.GetAll();
        }

        public void Add(Urun urun)
        {
            // 1. FluentValidation Çalıştırma
            UrunValidator validator = new UrunValidator();
            ValidationResult result = validator.Validate(urun);

            if (!result.IsValid)
            {
                // Hataları fırlatabilir veya bir liste şeklinde dönebilirsin
                string errorMessages = string.Join("\n", result.Errors);
                throw new Exception($"Doğrulama Hataları:\n{errorMessages}");
            }

            // 2. Ürün Kodu Zaten Var Mı? (Soft Delete yapılan silinmiş ürünleri hariç tutarak kontrol eder)
            var existing = _urunDal.Get(u => u.URUN_KOD == urun.URUN_KOD && u.SIL_TAR_ZMN == null);
            if (existing != null)
            {
                throw new Exception("Bu ürün kodu ile kayıtlı ürün zaten mevcut.");
            }

            // 3. Doğrulama başarılıysa veritabanı kayıt kodları çalışır
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

        public Urun GetById(Guid id)
        {
            return _urunDal.Get(u => u.URUN_ID == id);
        }
    }
}