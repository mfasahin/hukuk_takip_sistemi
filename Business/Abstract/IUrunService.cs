using Entity.Concrete;
using System;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IUrunService
    {
        List<Urun> GetAll();
        void Add(Urun urun);
        void Update(Urun urun);
        void Delete(Urun urun);
        Urun GetById(Guid id);
    }
}