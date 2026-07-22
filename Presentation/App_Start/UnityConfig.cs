using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace Presentation
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Business katmanı servisleri
            container.RegisterType<IIhtarService, IhtarManager>();
            container.RegisterType<IMusteriService, MusteriManager>();
            container.RegisterType<IAvukatService, AvukatManager>();
            container.RegisterType<ISubeService, SubeManager>();

            container.RegisterType<IUrunService, UrunManager>();
            container.RegisterType<IIhtarUrunService, IhtarUrunManager>();

            container.RegisterType<IIcraService, IcraManager>();
            container.RegisterType<IMahkemeService, MahkemeManager>();

            // DataAccess katmanı DAL sınıfları
            container.RegisterType<IIhtarDal, EfIhtarDal>();
            container.RegisterType<IMusteriDal, EfMusteriDal>();
            container.RegisterType<IAvukatDal, EfAvukatDal>();
            container.RegisterType<ISubeDal, EfSubeDal>();
            
            container.RegisterType<IUrunDal, EfUrunDal>();          
            container.RegisterType<IIhtarUrunDal, EfIhtarUrunDal>();

            container.RegisterType<IIcraDal, EfIcraDal>();            
            container.RegisterType<IMahkemeDal, EfMahkemeDal>();

            container.RegisterType<IKullaniciService, KullaniciManager>();
            container.RegisterType<IKullaniciDal, EfKullaniciDal>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}