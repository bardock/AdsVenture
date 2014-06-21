using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

namespace AdsVenture.Presentation.ContentServer.Helpers.Extensions
{
    public static class HttpActionDescriptorExtensions
    {
        public static bool RequiresAccessCode(this HttpActionDescriptor actionDescriptor)
        {
            var filter = actionDescriptor.GetCustomAttributes<Filters.AccessCodeAttribute>().LastOrDefault()
                ?? actionDescriptor.ControllerDescriptor.GetCustomAttributes<Filters.AccessCodeAttribute>().LastOrDefault();

            return filter != null;
        }

        public static bool IsResource<T>(this HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor.IsResource(typeof(T));
        }

        public static bool IsResource(this HttpActionDescriptor actionDescriptor, Type type)
        {
            return actionDescriptor.GetCustomAttributes<Filters.ResourceAttribute>()
                .Concat(actionDescriptor.ControllerDescriptor.GetCustomAttributes<Filters.ResourceAttribute>())
                .Any(x => x.Type == type);
        }
    }
}