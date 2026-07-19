using Entity.Concrete;
using System;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IMahkemeService
    {
        List<IcraMahkeme> GetAll();
        IcraMahkeme GetById(Guid id);
        void Add(IcraMahkeme mahkeme);
        void Update(IcraMahkeme mahkeme);
    }
}