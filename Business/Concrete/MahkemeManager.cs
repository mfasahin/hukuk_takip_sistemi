using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class MahkemeManager : IMahkemeService
    {
        private readonly IMahkemeDal _mahkemeDal;

        public MahkemeManager(IMahkemeDal mahkemeDal)
        {
            _mahkemeDal = mahkemeDal;
        }

        public List<IcraMahkeme> GetAll() => _mahkemeDal.GetAll();
        public IcraMahkeme GetById(Guid id) => _mahkemeDal.Get(m => m.MAHKEME_ID == id);
        public void Add(IcraMahkeme mahkeme) => _mahkemeDal.Add(mahkeme);
        public void Update(IcraMahkeme mahkeme) => _mahkemeDal.Update(mahkeme);
    }
}