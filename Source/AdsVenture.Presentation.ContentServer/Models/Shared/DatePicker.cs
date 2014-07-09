using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AdsVenture.Presentation.ContentServer.Models.Shared
{
    public class DatePicker
    {
        public string InputClass { get; set; }
        public bool AppendCalendarMode { get; set; }
        public bool Disabled { get; set; }
        public MinViewModes MinViewMode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Func<IDictionary<string, object>, MvcHtmlString> TextBoxBuilder { get; set; }

        public enum MinViewModes
        {
            Days,
            Months,
            Years
        }
    }
}