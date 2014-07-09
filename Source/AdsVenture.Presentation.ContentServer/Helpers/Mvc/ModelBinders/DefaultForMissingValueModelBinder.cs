using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace AdsVenture.Presentation.ContentServer.Helpers.Mvc.ModelBinders
{
    public class DefaultForMissingValueModelBinder<T> : DefaultValueModelBinder<T>
    {
        protected override bool RequiresDefautlValue(T obj, KeyValuePair<string, Func<T, object>> field, ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var val = bindingContext.ValueProvider.GetValue(field.Key);
            return val == null;
        }

        public static DefaultForMissingValueModelBinder<T> Register(ModelBinderDictionary binders)
        {
            var binder = new DefaultForMissingValueModelBinder<T>();
            binders.Add(typeof(T), binder);
            return binder;
        }

    }
}