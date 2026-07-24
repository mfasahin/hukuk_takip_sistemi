using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Linq;

namespace DataAccess.Concrete
{
    public class EfIhtarUrunDal : EfEntityRepositoryBase<IhtarUrun, AppDbContext>, IIhtarUrunDal
    {
        
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
        public bool UruneBagliIhtarVarMi(Guid urunId)
        {
            using (var context = new AppDbContext())
            {
                return context.IHTAR_URUN
                    .Any(iu => iu.URUN_ID == urunId && iu.SIL_TAR_ZMN == null);
            }
        }
    }
}
