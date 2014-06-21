using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdsVenture.Commons.Entities;

namespace AdsVenture.Core.Extensions.Entities
{
    public static class ICountryNavigatorExtensions
    {
        public static Country _Country(this ICountryNavigator e)
        {
            if (!e._CountryID.HasValue)
            {
                return null;
            }
            return Bootstrapper.Cache.Countries.GetData().FirstOrDefault(x => x.ID == e._CountryID);
        }
    }
}
