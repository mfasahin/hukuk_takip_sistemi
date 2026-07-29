using Entity.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IMusteriService
    {
        List<Musteri> GetAll();
        Musteri GetById(Guid id);
        void Add(Musteri musteri);
        void Update(Musteri musteri);
        void Delete(Musteri musteri);

        // Business katmanı saf veri tipleriyle çalışmalıdır (JsonResult kullanılmaz)
        string GenerateUniqueMustNo();
        ValidationResult Validate(Musteri musteri);
    }
}