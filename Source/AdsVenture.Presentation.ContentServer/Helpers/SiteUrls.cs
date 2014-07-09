using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace AdsVenture.Presentation.ContentServer.Helpers
{
    public static class SiteUrls
    {
        public static string ExcelExport(this UrlHelper helper)
        {
            var currentUrl = helper.RequestContext.HttpContext.Request.Url.PathAndQuery;
            var separator = "&";
            if ((currentUrl.EndsWith("?")))
            {
                separator = "";
            }
            else if ((!currentUrl.Contains("?")))
            {
                separator = "?";
            }
            return string.Format("{0}{1}export=excel", currentUrl, separator);
        }
    }
}