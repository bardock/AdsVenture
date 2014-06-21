using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using System.Runtime.CompilerServices;

namespace AdsVenture.Core.Validation
{

    public static class ValidatorExtensions
    {

        public static IRuleBuilderOptions<T, System.DateTime> Valid<T>(this IRuleBuilder<T, System.DateTime> ruleBuilder)
        {
            return ruleBuilder.GreaterThan(DateTime.MinValue).LessThan(DateTime.MaxValue);
        }

        public static IRuleBuilderOptions<T, System.DateTime?> Valid<T>(this IRuleBuilder<T, System.DateTime?> ruleBuilder)
        {
            return ruleBuilder.GreaterThan(DateTime.MinValue).LessThan(DateTime.MaxValue);
        }

        public static IRuleBuilderOptions<T, int> ValidID<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder.GreaterThan(0);
        }

        public static IRuleBuilderOptions<T, int?> ValidID<T>(this IRuleBuilder<T, int?> ruleBuilder)
        {
            return ruleBuilder.GreaterThan(0);
        }

        public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches("^(http|https|ftp)\\://([a-zA-Z0-9\\.\\-]+(\\:[a-zA-Z0-9\\.&%\\$\\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\\-]+\\.)*[a-zA-Z0-9\\-]+\\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(\\:[0-9]+)*(/($|[a-zA-Z0-9\\.\\,\\?\\'\\\\\\+&%\\$#\\=~_\\-]+))*$");
        }

    }

}
