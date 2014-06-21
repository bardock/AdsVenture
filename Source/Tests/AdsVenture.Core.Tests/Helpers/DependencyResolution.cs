using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using AdsVenture.Commons.Entities;
using AdsVenture.Commons.Helpers;
using AdsVenture.Core;
using Autofac;
using Autofac.Builder;
using AdsVenture.Core.Managers;

namespace AdsVenture.Core.Tests.Helpers
{
    public static class DependencyResolution
    {
        public static void AsManager<TLimit, TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            builder
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
        }

        //public static Mock<MediaObjectTypeManager> CreateMediaObjectTypeManagerMock(IComponentContext c)
        //{
        //    var mock = new Mock<MediaObjectTypeManager>(
        //        c.Resolve<Core.Helpers.IUnitOfWork>()
        //    ) { CallBase = true };
        //    return mock;
        //}
    }
}
