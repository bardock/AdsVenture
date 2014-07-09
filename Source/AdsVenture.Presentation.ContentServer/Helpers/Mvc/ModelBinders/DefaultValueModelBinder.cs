using Bardock.Utils.Web.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace AdsVenture.Presentation.ContentServer.Helpers.Mvc.ModelBinders
{
    public class DefaultValueModelBinder<T> : DefaultModelBinder
    {
        private Dictionary<string, Func<T, object>> _fields = new Dictionary<string, Func<T, object>>();

        public DefaultValueModelBinder() 
        { }

        public DefaultValueModelBinder<T> AddField(Expression<Func<T, object>> nameExpr, Func<T, object> value)
        {
            _fields.Add(Bardock.Utils.Linq.Expressions.ExpressionHelper.GetExpressionText(nameExpr), value);
            return this;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
	    {
		    T obj = (T)base.BindModel(controllerContext, bindingContext);
		    foreach (var field in _fields) {
			    if ((RequiresDefautlValue(obj, field, controllerContext, bindingContext))) {
				    var prop = obj.GetType().GetProperty(field.Key);
                    prop.GetSetMethod().Invoke(obj, new object[] { field.Value.Invoke(obj) });
			    }
		    }
		    return obj;
	    }

        protected virtual bool RequiresDefautlValue(T obj, KeyValuePair<string, Func<T, object>> field, ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var prop = obj.GetType().GetProperty(field.Key);
            var val = prop.GetGetMethod().Invoke(obj, null);
            return val == null;
        }

        public static DefaultValueModelBinder<T> Register(ref ModelBinderDictionary binders)
        {
            var binder = new DefaultValueModelBinder<T>();
            binders.Add(typeof(T), binder);
            return binder;
        }

    }
}