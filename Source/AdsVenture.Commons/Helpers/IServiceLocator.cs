using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Commons.Helpers
{
    public interface IServiceLocator
    {
        TService GetService<TService>();

        TReturn RunInNewScope<TReturn>(Func<TReturn> process);

        void RunInNewScope(Action process);
    }
}