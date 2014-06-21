using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdsVenture.Commons.Helpers;
using Autofac;

namespace AdsVenture.Core.Helpers
{
    public class AutofacServiceLocator : IServiceLocator
    {
        protected ILifetimeScope _container;
        public AutofacServiceLocator(ILifetimeScope container)
        {
            _container = container;
        }

        public TService GetService<TService>()
        {
            return _container.Resolve<TService>();
        }

        public TReturn RunInNewScope<TReturn>(Func<TReturn> process)
        {
            using ((_container.BeginLifetimeScope()))
            {
                return process();
            }
        }

        public void RunInNewScope(Action process)
        {
            RunInNewScope<bool>(() =>
            {
                process();
                return true;
            });
        }
    }
}