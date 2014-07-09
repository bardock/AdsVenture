using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdsVenture.Presentation.ContentServer.Filters.Api
{
    public class ApiAccessCodeAttribute : Bardock.Utils.Web.WebApi.Filters.AccessCodeAttribute
    {
        public ApiAccessCodeAttribute() : base()
        {
            //Filter.Code = Helpers.ConfigSection.Default.AccessCode.Value;
        }
    }
}