using Autofac;
using Bardock.Utils.Linq.Expressions;
using AdsVenture.Commons.Entities;
using AdsVenture.Core.Exceptions;
using AdsVenture.Core.Managers;
using AdsVenture.Core.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Ioc;

namespace AdsVenture.Core.Tests.Managers
{
    [RunWith(typeof(IocTestClassCommand))]
    [DependencyResolverBootstrapper(typeof(DependencyResolverBootstrapper))]
    public class UserManagerTest : _BaseManagerTest<UserManager>
    {
        public class DependencyResolverBootstrapper : AutofacDependencyResolverBootstrapper
        {
            protected override void OnRegistered(ContainerBuilder builder)
            {
                //builder.Register((c) =>
                //{
                //    var mock = DependencyResolution.CreateUserSubscriptionTypeManagerMock(c);
                //    return mock.Object;
                //})
                //.As<UserSubscriptionTypeManager>()
                //.AsManager();
            }

        }

        public UserManagerTest(
            TestFixture fixture,
            UserManager manager)
            : base(fixture, manager)
        {
        }
        

        //[Fact]
        public async Task Update()
        {
        }
    }
}
