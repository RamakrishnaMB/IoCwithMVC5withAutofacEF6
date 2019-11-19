using Autofac;
using Autofac.Integration.Mvc;
using BuisnessLayer.DependencyInjection;
using System;
using System.Web.Http;
using System.Web.Mvc;

namespace MVC5Web.Configuration
{
    public class ConfigureAutofac
    {
        public static void RegisterAutofac()
        {

            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            //below line for web api controllers 
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyModules(assemblies);
            //---------------
            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();
            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());
            //Other dependency in Bussiness layer
            builder.RegisterModule(new BusinessInjectionModule());
            var container = builder.Build();
            //below line for web api controllers 
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}