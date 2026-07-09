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

        public DbSet<Kullanıcı> Kullanıcılar { get; set; }

        public DbSet<Urun> Urunler { get; set; }

        public DbSet<Ihtar> Ihtarlar { get; set; }

        public DbSet<IhtarUrun> IhtarUrunler { get; set; }

        public DbSet<Icra> Icralar { get; set; }

        public DbSet<Mahkeme> IcraMahkemeler { get; set; }

    }
}
