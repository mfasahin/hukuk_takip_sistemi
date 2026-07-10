using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
