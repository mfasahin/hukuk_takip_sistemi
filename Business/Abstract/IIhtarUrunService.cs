using Entity.Concrete;
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
        void Delete(Guid id);
        IhtarUrun GetById(Guid id);

        List<IhtarUrun> GetByIhtarId(Guid ihtarId);

    }
}