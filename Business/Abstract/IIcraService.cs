using Entity.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IIcraService
    {
        List<Icra> GetAll();
        void Add(Icra icra);
        void Update(Icra icra);
        void Delete(Icra icra);
        Icra GetById(int id);
    }
}