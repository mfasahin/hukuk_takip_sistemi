using Entity.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IMahkemeService
    {
        List<Mahkeme> GetAll();
        Mahkeme GetById(int id);
        void Add(Mahkeme mahkeme);
        void Update(Mahkeme mahkeme);
    }
}