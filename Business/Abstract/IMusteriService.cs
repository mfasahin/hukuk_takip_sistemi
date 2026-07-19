using Entity.Concrete;
using System;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IMusteriService
    {
        List<Musteri> GetAll();
        void Add(Musteri musteri);
        void Update(Musteri musteri);
        void Delete(Musteri musteri);
        Musteri GetById(Guid id); 
    }
}