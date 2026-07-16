using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace Presentation.App_Start
{
    public static class DependencyResolverConfig
    {
        public static void RegisterDependencies()
        {
            var container = new UnityContainer();

            // IoC Container kayıtları
            container.RegisterType<IMusteriService, MusteriManager>();
            container.RegisterType<IMusteriDal, EfMusteriDal>();

            container.RegisterType<IAvukatService, AvukatManager>();
            container.RegisterType<IAvukatDal, EfAvukatDal>();

            container.RegisterType<IUrunService, UrunManager>();
            container.RegisterType<IUrunDal, EfUrunDal>();

            container.RegisterType<IIhtarService, IhtarManager>();
            container.RegisterType<IIhtarDal, EfIhtarDal>();

            container.RegisterType<IIcraService, IcraManager>();
            container.RegisterType<IIcraDal, EfIcraDal>();

            container.RegisterType<ISubeService, SubeManager>();
            container.RegisterType<ISubeDal, EfSubeDal>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
