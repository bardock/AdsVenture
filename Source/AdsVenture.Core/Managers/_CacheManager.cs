using System;
using System.Linq;
using AdsVenture.Commons.Entities;
using AdsVenture.Core.Extensions.Entities;

namespace AdsVenture.Core.Managers
{
    /// <summary>
    /// Singleton class that handles cache dependencies
    /// </summary>
    internal class _CacheManager
    {

	    #region Singleton

	    private static _CacheManager _instance;

	    public static _CacheManager Instance {
		    get 
            {
			    if (_instance == null) 
                {
				    _instance = new _CacheManager();
			    }
			    return _instance;
		    }
	    }

	    private _CacheManager() { }

	    #endregion

	    protected Data.CacheContext Cache 
        {
            get { return Bootstrapper.Cache; }
	    }

	    /// <summary>
	    /// Initialize events handlers in order to update cache.
	    /// This method must be invoked once in application init
	    /// and it is not necessary unsubscribe the handlers because they are application-wide.
	    /// </summary>
	    public void BindEvents()
        {
	    }
    }
}