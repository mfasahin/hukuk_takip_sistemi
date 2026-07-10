using Core.DataAccess;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
