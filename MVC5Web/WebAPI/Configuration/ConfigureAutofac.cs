using Autofac;
using Autofac.Integration.WebApi;
using BuisnessLayer.DependencyInjection;
using System;
using System.Reflection;
using System.Web.Http;

namespace WebAPI.Configuration
{
    public class ConfigureAutofac
    {
        public static void RegisterAutofac()
        {

            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            //below line for web api controllers 
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyModules(AppDomain.CurrentDomain.GetAssemblies());
            // Register dependencies in controllers
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
            // Register dependencies in filter attributes
            builder.RegisterWebApiFilterProvider(config);
            //Other dependency in Bussiness layer
            builder.RegisterModule(new BusinessInjectionModule());
            var container = builder.Build();
            //below line for web api controllers 
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}