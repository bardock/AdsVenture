using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using AdsVenture.Commons;

namespace AdsVenture.Core.Validation
{

    public class ValidationResolvers
    {

        public static string DisplayNameResolver(Type type, MemberInfo member, System.Linq.Expressions.LambdaExpression expression)
        {
            if (member == null)
            {
                return null;
            }
            //Check display attributes
            var attrs = member.GetCustomAttributes(typeof(DisplayNameAttribute));
            if ((attrs != null && attrs.Count() > 0))
            {
                return ((DisplayNameAttribute) attrs.First()).DisplayName;
            }
            attrs = member.GetCustomAttributes(typeof(DisplayAttribute));
            if ((attrs != null && attrs.Count() > 0))
            {
                return ((DisplayAttribute) attrs.First()).GetName();
            }
            return member.Name;
        }

    }

}
