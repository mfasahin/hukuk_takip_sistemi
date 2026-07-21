using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IIhtarUrunService
    {
        List<IhtarUrun> GetAll();
        void Add(IhtarUrun ihtarUrun);
        void Update(IhtarUrun ihtarUrun);
        void Delete(IhtarUrun ihtarUrun);
        IhtarUrun GetById(Guid id);
        
    }
}