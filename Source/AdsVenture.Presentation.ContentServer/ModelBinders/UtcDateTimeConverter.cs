using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AdsVenture.Presentation.ContentServer.ModelBinders
{
    public class UtcDateTimeConverter : DateTimeConverter
    {
        /// <summary>
        /// If date comes in UTC (ends with 'Z') then a DateTime with Utc Kind will be created.
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null || !(value is string) || !value.ToString().EndsWith("Z"))
                return base.ConvertFrom(context, culture, value);

            var json = string.Format("\"{0}\"", value.ToString());
            return JsonConvert.DeserializeObject<DateTime>(json);
        }
    }
}