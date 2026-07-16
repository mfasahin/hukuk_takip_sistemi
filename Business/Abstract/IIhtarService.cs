using Entity.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IIhtarService
    {
        List<Ihtar> GetAll();
        void Add(Ihtar ihtar);
        void Update(Ihtar ihtar);
        void Delete(Ihtar ihtar);
        Ihtar GetById(int id);

        List<Ihtar> GetIhtarWithRelations();
    }
}