using System;
using System.Collections.Generic;
using System.Linq;
using AdsVenture.Commons.Caching;
using AdsVenture.Commons.Entities;
using AdsVenture.Data.Helpers;
using Sixeyed.Caching;

namespace AdsVenture.Data
{
    public class CacheContext
    {
        #region "PrivateMembers"

        private static readonly TimeSpan EXPIRATION_DEFAULT = TimeSpan.FromMinutes(ConfigSection.Default.CacheContext.MinutesDuration);
        private static readonly string CACHE_KEY_PREFIX = ConfigSection.Default.CacheContext.Prefix;

        private Data.DataContext _db;
        private ICache _cache;

        #endregion

        public CacheContext(Data.DataContext db, ICache cache)
        {
	        _db = db;
            _cache = cache;

            _countriesProxy = new CacheProxy<List<Country>>(CountriesDataLoad, _cache, CACHE_KEY_PREFIX + "Countries", EXPIRATION_DEFAULT);

            _languagesProxy = new CacheProxy<List<Language>>(LanguagesDataLoad, _cache, CACHE_KEY_PREFIX + "Languages", EXPIRATION_DEFAULT);
        }

        #region "HelperMethods"

        private Data.DataContext Db { 
            get { return _db; } 
        }

        #endregion

        #region Countries

        private CacheProxy<List<Country>> _countriesProxy;

        private List<Country> CountriesDataLoad()
        {
            return Db.Countries.OrderBy(x => x.Description).ToList();
        }

        public CacheProxy<List<Country>> Countries
        {
            get { return _countriesProxy; }
        }

        #endregion

        #region Languages

        private CacheProxy<List<Language>> _languagesProxy;
        private List<Language> LanguagesDataLoad()
        {
            return Db.Languages.ToList();
        }

        public CacheProxy<List<Language>> Languages
        {
            get { return _languagesProxy; }
        }

        #endregion
    }
}
