using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AdsVenture.Core.Modules;

namespace AdsVenture.Presentation.ContentServer
{
    public class DependenciesConfig
    {
        public static void Register(System.Web.Http.HttpConfiguration config)
        {
            var builder = new Autofac.ContainerBuilder();
            
            builder.RegisterModule(new ConfigurationSettingsReader());

            builder.RegisterModule(new Core.Modules.ManagersModule());

            builder.RegisterModule(new DataModule());

            builder.RegisterModule(new CommonsModule());

            //Cache
            builder
                .RegisterType<Sixeyed.Caching.Serialization.Serializers.Json.JsonSerializer>()
                .As<Sixeyed.Caching.Serialization.Serializers.IJsonSerializer>();
            builder
                .RegisterType<Sixeyed.Caching.Serialization.Serializer>()
                .PropertiesAutowired()
                .AsSelf();

            //Controllers
            builder.RegisterControllers(typeof(WebApiApplication).Assembly);
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            //Filters
            builder.RegisterFilterProvider();
            builder.RegisterWebApiFilterProvider(config);

            var container = builder.Build();

            //Mvc DependencyResolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //WebApi DependencyResolver
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}