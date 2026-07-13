using Entity.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IAvukatService
    {
        List<Avukat> GetAll();
        void Add(Avukat avukat);
        void Update(Avukat avukat);
        void Delete(Avukat avukat);
        Avukat GetById(int id);
    }
}
