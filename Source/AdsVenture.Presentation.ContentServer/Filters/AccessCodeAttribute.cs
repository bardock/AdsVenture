using AdsVenture.Presentation.ContentServer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AdsVenture.Presentation.ContentServer.Filters
{
    public class AccessCodeAttribute : Bardock.Utils.Web.Mvc.Filters.HttpAccessCodeAttribute
    {
        public AccessCodeAttribute()
            : base()
        {
            this.ParamName = ConfigSection.Default.AccessCodeConfiguration.ParamName;
            this.Code = ConfigSection.Default.AccessCodeConfiguration.Code;
        }
    }
}