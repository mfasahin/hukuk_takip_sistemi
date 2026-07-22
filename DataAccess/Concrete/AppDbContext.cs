using Core;
using Entity.Abstract;
using Entity.Concrete;
using System;
using System.Data.Entity;
using System.Runtime.InteropServices;

namespace DataAccess.Concrete
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {

        }
        public DbSet<Musteri> MUSTERI { get; set; }

        public DbSet<Avukat> AVUKAT { get; set; }

        public DbSet<Sube> SUBE { get; set; }

        public DbSet<Kullanici> KULLANICI { get; set; }

        public DbSet<Urun> URUN { get; set; }

        public DbSet<Ihtar> IHTAR { get; set; }

        public DbSet<IhtarUrun> IHTAR_URUN { get; set; }

        public DbSet<Icra> ICRA { get; set; }

        public DbSet<IcraMahkeme> ICRA_MAHKEME { get; set; }
    

    public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.GRS_TAR_ZMN = DateTime.Now;
                    entry.Entity.GRS_KULLANICI_ID = CurrentUser.UserId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.GNC_TAR_ZMN = DateTime.Now;
                    entry.Entity.GNC_KULLANICI_ID = CurrentUser.UserId;

                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified; // soft delete
                    entry.Entity.SIL_TAR_ZMN = DateTime.Now;
                    entry.Entity.SIL_KULLANICI_ID = CurrentUser.UserId;

                }
            }

            return base.SaveChanges();
        }
    }
}