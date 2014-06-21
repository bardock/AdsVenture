using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdsVenture.Commons.Helpers;

namespace AdsVenture.Core.Tests
{
    /// <summary>
    /// Handles the global initialization/teardown functionality.
    /// </summary>
    public class TestFixture : IDisposable
    {
        public const int DEFAULT_USER_ID = 2;

        public TestFixture(IServiceLocator serviceLocator)
        {
            if (!Bootstrapper.IsInitialized)
                Bootstrapper.Init(serviceLocator);
        }

        public void Dispose() { }


        public string GetString(int count)
        {
            var rand = new Random();

            var guid = Guid.NewGuid().ToString("n");
            var guidLength = guid.Length;

            return new string(
                new char[count]
                    .Select(c => guid[rand.Next(guidLength)])
                    .ToArray()
            );
        }
    }
}
