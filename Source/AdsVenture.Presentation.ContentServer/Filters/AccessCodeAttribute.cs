using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdsVenture.Presentation.ContentServer.Filters
{
    public class AccessCodeAttribute : Bardock.Utils.Web.Mvc.Filters.AccessCodeAttribute
    {
        public AccessCodeAttribute() : base()
        {
            //Filter.Code = Helpers.ConfigSection.Default.AccessCode.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}