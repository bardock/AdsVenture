using Autofac;
using System;
using System.Collections.Generic;
using Xunit.Ioc;
using Xunit.Ioc.Autofac;
using Autofac.Configuration;

namespace AdsVenture.Core.Tests
{
    public class AutofacDependencyResolverBootstrapper : IDependencyResolverBootstrapper
    {
        private static readonly object _lock = new object();
        private static Dictionary<Type, IDependencyResolver> _dependencyResolvers = new Dictionary<Type, IDependencyResolver>();

        public IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new ConfigurationSettingsReader());

            builder.RegisterModule(new Core.Modules.ManagersModule());

            builder.RegisterModule(
                new Core.Modules.DataModule() { Persistant = false });

            builder.RegisterModule(
                new Core.Modules.CommonsModule() { Persistant = false });

            //Tests
            builder.RegisterType<TestFixture>().SingleInstance();
            builder.RegisterModule(new TestsModule(typeof(AutofacDependencyResolverBootstrapper).Assembly));

            OnRegistered(builder);

            return builder.Build();
        }

        protected virtual void OnRegistered(ContainerBuilder builder) { }

        public virtual IDependencyResolver GetResolver()
        {
            lock (_lock)
            {
                if (_dependencyResolvers.ContainsKey(this.GetType()) == false)
                    _dependencyResolvers[this.GetType()] = new AutofacDependencyResolver(CreateContainer());
                return _dependencyResolvers[this.GetType()];
            }
        }
    }
}
