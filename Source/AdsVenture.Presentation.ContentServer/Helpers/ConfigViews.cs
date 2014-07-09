using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.CompilerServices;

namespace AdsVenture.Presentation.ContentServer.Helpers
{
    public static class ConfigViews
    {
        public static Model Config<TModel>(this System.Web.Mvc.HtmlHelper<TModel> helper)
        {
            return new Model
            {
                Core = Core.Helpers.ConfigSection.Default,
                WebSite = ConfigSection.Default
            };
        }

        public class Model
        {
            public Core.Helpers.ConfigSection Core { get; set; }
            public ConfigSection WebSite { get; set; }
        }
    }
}