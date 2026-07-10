using System.Data.Entity;
using Entity.Concrete;

namespace DataAccess.Concrete
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {
            
        }


        public DbSet<Musteri> Musteri { get; set; }

        public DbSet<Avukat> AVUKAT { get; set; }

        public DbSet<Sube> SUBE { get; set; }

        public DbSet<Kullanici> KULLANICI { get; set; }

        public DbSet<Urun> URUN { get; set; }

        public DbSet<Ihtar> IHTAR { get; set; }

        public DbSet<IhtarUrun> IHTAR_URUN { get; set; }

        public DbSet<Icra> ICRA { get; set; }
        
        public DbSet<Mahkeme> ICRA_MAHKEME { get; set; }

    }
}
