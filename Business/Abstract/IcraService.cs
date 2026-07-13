using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IcraService
    {
        List<Icra> GetAll();
        void Add(Icra icra);
        void Update(Icra icra);
        void Delete(Icra icra);
        Icra GetById(int id);
    }
}
