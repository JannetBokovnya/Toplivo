using System.Web.Mvc;
using Toplivo.Web.Models.Toplivo;
using Toplivo.Web.Repositories;
using Toplivo.Web.Services;
using Toplivo.Web.Controllers;
using Unity;
using Unity.Mvc5;
using Unity.Injection;

namespace Toplivo.Web
{
    public static class UnityConfig
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IToplivoRepository<Tank>, ToplivoRepository<Tank>>();
            container.RegisterType<IToplivoService<Tank>, TankService>();

            container.RegisterType<IToplivoRepository<Fuel>, ToplivoRepository<Fuel>>();
            container.RegisterType<IToplivoService<Fuel>, FuelService>();

            container.RegisterType<IToplivoRepository<Operation>, ToplivoRepository<Operation>>();
            container.RegisterType<IToplivoService<Operation>, OperationService>();

            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());

            RegisterTypes(container);
            return container;
        }
        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}