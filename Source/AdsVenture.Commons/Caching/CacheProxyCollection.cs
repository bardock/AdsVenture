using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixeyed.Caching;

namespace AdsVenture.Commons.Caching
{
    /// <summary>
    /// This class manages a set of proxies with same data type. 
    /// It identifies each proxy building a key by given variable params
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks></remarks>
    public class CacheProxyCollection<T> : DeferredCacheProxyCollection<T>
    {
        private Func<object[], Func<T>> _dataLoadFunc;

        public CacheProxyCollection(Func<object[], Func<T>> dataLoadFunc, ICache cache, string keyPrefix, TimeSpan expiration = default(TimeSpan))
            : this(dataLoadFunc, cache, keyPrefix, x => expiration) { }

        public CacheProxyCollection(Func<object[], Func<T>> dataLoadFunc, ICache cache, string keyPrefix, Func<T, TimeSpan> expiration)
            : base(cache, keyPrefix, expiration)
        {
            _dataLoadFunc = dataLoadFunc;
        }

        /// <summary>
        /// Get proxy data by specified params
        /// </summary>
        /// <remarks>
        /// You must specified the @params in the order expected by dataLoadFunc
        /// </remarks>
        public T GetData(params object[] @params)
        {
            return base.GetData(_dataLoadFunc(@params), @params);
        }

    }
}