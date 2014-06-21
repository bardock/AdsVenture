using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AdsVenture.Data.Helpers
{
    [DataContract()]
    public class ConfigSection : ConfigurationSection
    {
        [IgnoreDataMember()]
        public static ConfigSection Default
        {
            get { return (ConfigSection)ConfigurationManager.GetSection("adsVenture/data"); }
        }

        [DataMember()]
        [ConfigurationProperty("cacheContext")]
        public CacheContextConfigurationElement CacheContext
        {
            get { return (CacheContextConfigurationElement)this["cacheContext"]; }
            set { this["cacheContext"] = value; }
        }

        [DataContract()]
        public class CacheContextConfigurationElement : ConfigurationElement
        {

            [DataMember()]
            [ConfigurationProperty("minutesDuration", IsRequired = true)]
            public Int32 MinutesDuration
            {
                get { return (Int32)this["minutesDuration"]; }
                set { this["minutesDuration"] = value; }
            }

            [DataMember()]
            [ConfigurationProperty("prefix", IsRequired = true)]
            public string Prefix
            {
                get { return (string)this["prefix"]; }
                set { this["prefix"] = value; }
            }

        }
    }
}
