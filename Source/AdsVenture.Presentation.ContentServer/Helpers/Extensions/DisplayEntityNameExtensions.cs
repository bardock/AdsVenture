using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AdsVenture.Presentation.ContentServer.Helpers.Extensions
{
    public static class DisplayEntityNameExtensions
    {
        public static MvcHtmlString DisplayEntityName(this HtmlHelper html, string expression)
        {
            return new MvcHtmlString(Bardock.Utils.Globalization.Resources.Current.GetValue(expression));
        }

        public static MvcHtmlString DisplayEntityNameFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.DisplayEntityName(ExpressionHelper.GetExpressionText(expression));
        }

        public static MvcHtmlString DisplayEntityNameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.DisplayEntityName(ExpressionHelper.GetExpressionText(expression));
        }

        public static MvcHtmlString DisplayEntityNameForModel(this HtmlHelper html)
        {
            return html.DisplayEntityName(html.ViewData.ModelMetadata.ModelType.Name);
        }
    }
}