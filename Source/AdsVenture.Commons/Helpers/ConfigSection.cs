using System.Configuration;
using System.Runtime.Serialization;

namespace AdsVenture.Commons.Helpers
{
    [DataContract()]
    public class ConfigSection : ConfigurationSection
    {

        [IgnoreDataMember()]
        public static ConfigSection Default
        {
            get { return (ConfigSection)ConfigurationManager.GetSection("adsVenture/commons"); }
        }
    }
}