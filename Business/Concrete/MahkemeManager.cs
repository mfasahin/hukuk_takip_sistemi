using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
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

        public List<Mahkeme> GetAll() => _mahkemeDal.GetAll();
        public Mahkeme GetById(int id) => _mahkemeDal.Get(m => m.MAHKEME_ID == id);
        public void Add(Mahkeme mahkeme) => _mahkemeDal.Add(mahkeme);
        public void Update(Mahkeme mahkeme) => _mahkemeDal.Update(mahkeme);
    }
}