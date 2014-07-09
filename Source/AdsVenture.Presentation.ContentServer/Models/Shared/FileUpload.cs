using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AdsVenture.Presentation.ContentServer.Models.Shared
{
    public class FileUpload
    {
        public string ID { get; set; }
        public string Url { get; set; }
        public string UrlFieldName { get; set; }
        public string IsEmptyFieldName { get; set; }
        public Func<IDictionary<string, object>, MvcHtmlString> TextBoxBuilder { get; set; }

    }
}