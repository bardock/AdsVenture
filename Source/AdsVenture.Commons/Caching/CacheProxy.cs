using System;
using Sixeyed.Caching;

namespace AdsVenture.Commons.Caching
{
    public class CacheProxy<T> : DeferredCacheProxy<T>
    {
        private Func<T> _dataLoadFunc;

        public CacheProxy(Func<T> dataLoadFunc, ICache cache, string key, TimeSpan expiration = default(TimeSpan))
            : this(dataLoadFunc, cache, key, x => expiration) { }

        public CacheProxy(Func<T> dataLoadFunc, ICache cache, string key, Func<T, TimeSpan> expiration)
            : base(cache, key, expiration)
        {
            _dataLoadFunc = dataLoadFunc;
        }

        public T GetData()
        {
            return base.GetData(_dataLoadFunc);
        }
    }
}
    