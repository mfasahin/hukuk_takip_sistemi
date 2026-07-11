using Entity.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IhtarService
    {
        List<Ihtar> GetAll();
        void Add(Ihtar Ihtar);
        void Update(Ihtar Ihtar);
        void Delete(Ihtar Ihtar);
        Ihtar GetById(int id);
    }
}
