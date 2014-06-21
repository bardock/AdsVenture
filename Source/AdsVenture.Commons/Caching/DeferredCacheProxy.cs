using System;
using System.Web;
using System.Web.Caching;
using Bardock.Utils.Sync;
using Sixeyed.Caching;

namespace AdsVenture.Commons.Caching
{
    public class DeferredCacheProxy<T>
    {
        public static readonly TimeSpan EXPIRATION_DEFAULT = TimeSpan.FromHours(2);

        protected ICache _cache;
        protected string _key;
        protected Func<T, TimeSpan> _expiration;

        public DeferredCacheProxy(ICache cache, string key, TimeSpan expiration = default(TimeSpan))
            : this(cache, key, x => expiration) { }

        public DeferredCacheProxy(ICache cache, string key, Func<T, TimeSpan> expiration)
        {
            _cache = cache;
            if (key == null)
                throw new ArgumentException("key cannot be null");
            _key = key;
            _expiration = expiration;
        }
        
        public T GetData(Func<T> dataLoadFunc)
        {
            T data;
            try
            {
                data = _cache.Get<T>(_key);
            }
            catch (CacheKeyNotFoundException)
            {
                lock (_locker.GetLockObject(_key))
                {
                    data = dataLoadFunc();
                    _cache.Set(_key, data, GetExpiration(data));
                }
            }
            return data;
        }

        protected TimeSpan GetExpiration(T obj)
        {
            var timespan = _expiration(obj);
            return timespan == default(TimeSpan) ? EXPIRATION_DEFAULT : timespan;
        }

        public void Clear()
        {
            _cache.Remove(_key);
        }

        private static StringLocker _locker = new StringLocker();
    }
}
    
