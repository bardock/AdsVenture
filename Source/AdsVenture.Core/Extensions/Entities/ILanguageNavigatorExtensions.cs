using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdsVenture.Commons.Entities;

namespace AdsVenture.Core.Extensions.Entities
{
    public static class ILanguageNavigatorExtensions
    {
        public static Language _Language(this ILanguageNavigator e)
        {
            return Bootstrapper.Cache.Languages.GetData().FirstOrDefault(x => x.ID == e._LanguageID);
        }
    }
}
