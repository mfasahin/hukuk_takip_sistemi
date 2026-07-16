using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISubeService
    {
        List<Sube> GetAll();
        void Add(Sube sube);
        void Update(Sube sube);
        void Delete(Sube sube);
        Sube GetById(int id);
    }
}
