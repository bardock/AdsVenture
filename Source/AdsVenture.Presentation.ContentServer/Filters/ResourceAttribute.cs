using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdsVenture.Presentation.ContentServer.Filters
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class ResourceAttribute : Attribute
    {
        public Type Type { get; set; }

        public ResourceAttribute(Type type)
        {
            this.Type = type;
        }
    }
}