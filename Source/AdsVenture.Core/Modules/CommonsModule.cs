using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using AdsVenture.Commons;

namespace AdsVenture.Core.Modules
{
    public class CommonsModule : Module
    {
        public bool Persistant { get; set; }

        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder
                .RegisterType<Helpers.AutofacServiceLocator>()
                .As<Commons.Helpers.IServiceLocator>()
                .InstancePerLifetimeScope();
        }
    }
}
