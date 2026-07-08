using System.Data.Entity;
using Entity.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()  : base("DefaultConnection")
        {
            
        }


        public DbSet<Musteri> Musteriler { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Musteri>()
                .HasKey(e => e.MUSTERI_ID);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.MUST_NO)
                .IsRequired()
                .HasMaxLength(8);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.MUST_AD)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.MUST_SOYAD)
                .HasMaxLength(25);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.MUST_KIMLIK_NO)
                .HasMaxLength(11);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.MUST_VKNO)
                .HasMaxLength(10);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.MUST_EPOSTA)
                .HasMaxLength(50);
        }
    }
}
