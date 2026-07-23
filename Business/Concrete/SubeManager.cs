using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class SubeManager : ISubeService
    {
        private readonly ISubeDal _subeDal;

        // Constructor: Dal enjekte ediliyor
        public SubeManager(ISubeDal subeDal)
        {
            _subeDal = subeDal;
        }

        // Tüm şubeleri getir
        public List<Sube> GetAll()
        {
            return _subeDal.GetAll();
        }

        // Id’ye göre şube getir
        public Sube GetById(Guid id)
        {
            return _subeDal.Get(s => s.SUBE_ID == id);
        }

        // Yeni şube ekle
        public void Add(Sube sube)
        {
            _subeDal.Add(sube);
        }

        // Şube güncelle
        public void Update(Sube sube)
        {
            _subeDal.Update(sube);
        }

        // Şube sil
        public void Delete(Sube sube)
        {
            _subeDal.Delete(sube);
        }

    }
}
