using Autofac;
using BuisnessLayer.Implementation;
using BuisnessLayer.Interface;
using DataLayer;
using DataLayer.Implementation;
using DataLayer.Interface;

namespace BuisnessLayer.DependencyInjection
{
    public class BusinessInjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(a => new SchoolEntities()).InstancePerRequest();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerRequest();
            builder.RegisterType<DashBoardFeeServices>().As<IDashBoardFeeServices>().InstancePerRequest().PropertiesAutowired();
        }
    }
}
