using System.Data.Entity;
using Entity.Concrete;

namespace DataAccess.Concrete
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {
            
        }


        public DbSet<Musteri> Musteriler { get; set; }

        public DbSet<Avukat> Avukatlar { get; set; }

        public DbSet<Sube> Subeler { get; set; }


    }
}
