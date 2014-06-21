using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace AdsVenture.Core.Modules
{
    public class DataModule : Module
    {
        public bool Persistant { get; set; }

        public DataModule()
        {
            Persistant = true;
        }

        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder
                .RegisterType<Helpers.UnitOfWork>()
                .As<Core.Helpers.IUnitOfWork>()
                .InstancePerLifetimeScope();
            if (this.Persistant)
            {
                builder
                    .RegisterType<Data.DataContext>()
                    .AsSelf()
                    .InstancePerLifetimeScope();
            }
            else
            {
                builder
                    .Register(c => {
                        var loader = new Effort.DataLoaders.CsvDataLoader(ConfigurationManager.AppSettings["Data.Path"]);
                        var connection = Effort.DbConnectionFactory.CreateTransient(loader);
                        return new Data.DataContext(connection);
                    })
                    .As<Data.DataContext>()
                    .InstancePerLifetimeScope();
            }

            builder
                .RegisterType<Data.CacheContext>().AsSelf()
                .InstancePerLifetimeScope();
        }

        protected void RegisterUnitOfWork<T>(Autofac.ContainerBuilder builder) where T : Helpers.IUnitOfWork
        {
        }
    }
}