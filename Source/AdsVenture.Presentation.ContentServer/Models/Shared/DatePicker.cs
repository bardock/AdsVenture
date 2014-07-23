using HtmlTags;
using System;

namespace AdsVenture.Presentation.ContentServer.Models.Shared
{
    public class DatePicker
    {
        public bool AppendCalendarMode { get; set; }
        public bool Disabled { get; set; }
        public MinViewModes MinViewMode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public HtmlTag TextBoxTag { get; set; }

        public enum MinViewModes
        {
            Days,
            Months,
            Years
        }
    }
}