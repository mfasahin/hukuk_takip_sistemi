using Entity.Concrete;
using System;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IAvukatService
    {
        List<Avukat> GetAll();
        void Add(Avukat avukat);
        void Update(Avukat avukat);
        void Delete(Avukat avukat);
        Avukat GetById(Guid id);
    }
}