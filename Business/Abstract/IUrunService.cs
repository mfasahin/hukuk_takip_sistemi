using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUrunService
    {
        List<Urun> GetAll();
        void Add(Urun urun);
        void Update(Urun urun);
        void Delete(Urun urun);
        Urun GetById(int id);
    }
}
