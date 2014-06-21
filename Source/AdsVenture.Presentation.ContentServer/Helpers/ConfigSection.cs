using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Runtime.Serialization;

namespace AdsVenture.Presentation.ContentServer.Helpers
{
    [DataContract()]
    public class ConfigSection : ConfigurationSection
    {
        [IgnoreDataMember()]
        public static ConfigSection Default
        {
            get { return (ConfigSection)ConfigurationManager.GetSection("adsVenture/presentation/contentServer"); }
        }

        [IgnoreDataMember]
        [ConfigurationProperty("accessCode")]
        public AccessCodeConfigurationElement AccessCodeConfiguration
        {
            get { return (AccessCodeConfigurationElement)this["accessCode"]; }
            set { this["accessCode"] = value; }
        }

        [DataContract]
        public class AccessCodeConfigurationElement : ConfigurationElement
        {
            [DataMember]
            [ConfigurationProperty("code", IsRequired = true)]
            public string Code
            {
                get { return (string)this["code"]; }
                set { this["code"] = value; }
            }

            [DataMember]
            [ConfigurationProperty("paramName", IsRequired = true)]
            public string ParamName
            {
                get { return (string)this["paramName"]; }
                set { this["paramName"] = value; }
            }
        }
    }
}