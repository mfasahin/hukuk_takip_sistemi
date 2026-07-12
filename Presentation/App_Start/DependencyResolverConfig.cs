using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Presentation.App_Start
{
    public static class DependencyResolverConfig
    {
        public static void RegisterDependencies()
        {
            var services = new ServiceCollection();

            // IoC Container kayıtları
            services.AddScoped<IMusteriService, MusteriManager>();
            services.AddScoped<IMusteriDal, EfMusteriDal>();

            services.AddScoped<IAvukatService, AvukatManager>();
            services.AddScoped<IAvukatDal, EfAvukatDal>();

            var provider = services.BuildServiceProvider();
            DependencyResolver.SetResolver(new DefaultDependencyResolver(provider));
        }
    }

    public class DefaultDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _provider;

        public DefaultDependencyResolver(IServiceProvider provider)
        {
            _provider = provider;
        }

        public object GetService(Type serviceType)
        {
            return _provider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _provider.GetServices(serviceType);
        }
    }
}
