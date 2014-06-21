using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace AdsVenture.Core.Helpers
{
    [DataContract()]
    public class ConfigSection : ConfigurationSection
    {

        [IgnoreDataMember]
        public static ConfigSection Default
        {
            get { return (ConfigSection)ConfigurationManager.GetSection("adsVenture/core"); }
        }

        [DataMember]
        [ConfigurationProperty("concurrentActionContext")]
        public ConcurrentActionContextConfigurationElement ConcurrentActionContext
        {
            get { return (ConcurrentActionContextConfigurationElement)this["concurrentActionContext"]; }
        }

        [DataContract]
        public class ConcurrentActionContextConfigurationElement : ConfigurationElement
        {

            [DataMember]
            [ConfigurationProperty("defaultMaxAttemps", IsRequired = true)]
            public int DefaultMaxAttemps
            {
                get { return (int)this["defaultMaxAttemps"]; }
            }
        }
    }
}