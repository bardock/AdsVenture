using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdsVenture.Core.Tests.Managers
{
    public abstract class _BaseManagerTest<TManager>
    {
        protected TestFixture _fixture;
        protected TManager _manager;

        public _BaseManagerTest(
            TestFixture fixture,
            TManager manager)
        {
            _fixture = fixture;
            _manager = manager;
        }

        protected void AssertAllEqual<T>(IEnumerable<T> e)
        {
            Assert.Equal(e, Enumerable.Repeat(e.First(), e.Count()));
        }
    }
}
