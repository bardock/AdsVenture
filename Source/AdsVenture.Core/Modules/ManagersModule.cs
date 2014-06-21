using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdsVenture.Commons;
using Autofac;
using Autofac.Core;

namespace AdsVenture.Core.Modules
{
    public class ManagersModule : Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(Core.Managers.IManager).Assembly)
                .AssignableTo<Core.Managers.IManager>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
        }
    }
}