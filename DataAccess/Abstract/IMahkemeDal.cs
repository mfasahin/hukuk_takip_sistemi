using Core.DataAccess;
using Entity.Concrete;

namespace DataAccess.Abstract
{
    public interface IMahkemeDal : IEntityRepository<IcraMahkeme>
    {
        // Mahkeme'nin özel bir sorguya ihtiyacı yok, generic repository yeterli
    }
}