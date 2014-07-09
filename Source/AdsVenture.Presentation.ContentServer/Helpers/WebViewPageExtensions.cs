using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Specialized;

namespace AdsVenture.Presentation.ContentServer.Helpers
{
    public static class WebViewPageExtensions
    {
        public static RouteValueDictionary GetRouteValues(this WebViewPage view, NameValueCollection coll)
        {
            return new RouteValueDictionary(coll.AllKeys.ToDictionary(x => x, x => (object)coll[x]));
        }
    }
}