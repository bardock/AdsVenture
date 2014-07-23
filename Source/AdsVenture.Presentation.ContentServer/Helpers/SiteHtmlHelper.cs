using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Bardock.Utils.Extensions;
using Bardock.Utils.Web.Mvc.Helpers;
using Bardock.Utils.Web.Mvc.Extensions;
using Bardock.Utils.Web.Mvc.HtmlTags.Extensions;
using Bardock.Utils.Web.Mvc.HtmlTags;
using HtmlTags;

namespace AdsVenture.Presentation.ContentServer.Helpers
{
    public static class SiteHtmlHelper
    {
        public static MvcHtmlString DatePicker(
            this HtmlHelper htmlHelper, 
            string name, 
            DateTime? value = null, string inputClass = null, bool appendCalendarMode = true, bool disabled = false)
        {
            return htmlHelper.Partial("~/Views/Shared/_DatePicker.cshtml", new Models.Shared.DatePicker
            {
                AppendCalendarMode = appendCalendarMode,
                Disabled = disabled,
                TextBoxTag = htmlHelper.Tags()
                    .TextBox(name)
                    .AddClass("input-small " + inputClass)
                    .Val(value.ApplyOrDefault(x => x.DateFormat()))
                    .Disabled(disabled)
            });
        }

        public static MvcHtmlString DatePickerFor<TModel>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, System.DateTime?>> expression, 
            System.DateTime? defaultValue = null, 
            string inputClass = null, 
            bool appendCalendarMode = true, 
            bool disabled = false, 
            Models.Shared.DatePicker.MinViewModes minViewMode = Models.Shared.DatePicker.MinViewModes.Days, 
            DateTime? startDate = null, 
            DateTime? endDate = null)
        {
            System.DateTime? value = defaultValue;
            if (htmlHelper.ViewData.Model != null)
            {
                value = expression.Compile().Invoke(htmlHelper.ViewData.Model);
            }
            var model = new Models.Shared.DatePicker
            {
                AppendCalendarMode = appendCalendarMode,
                Disabled = disabled,
                MinViewMode = minViewMode,
                StartDate = startDate,
                EndDate = endDate
            };
            if (expression.Body.NodeType == ExpressionType.Convert 
                || expression.Body.NodeType == ExpressionType.ConvertChecked)
            {
                model.TextBoxTag = htmlHelper.Tags()
                    .TextBoxFor(expression: (Expression<System.Func<TModel, System.DateTime>>)
                            Bardock.Utils.Linq.Expressions.ExpressionHelper.RemoveConvert(expression));
            }
            else
            {
                model.TextBoxTag = htmlHelper.Tags().TextBoxFor(expression);
            }

            model.TextBoxTag
                .AddClass("input-small " + inputClass)
                .Data("default-value", defaultValue.ApplyOrDefault(x => x.DateFormat()))
                .Val(value.ApplyOrDefault(x => x.DateFormat()))
                .Disabled(disabled);

            return htmlHelper.Partial("~/Views/Shared/_DatePicker.cshtml", model);
        }

        public static HtmlTag Select2For<TModel, TProperty>(
            this HtmlTagHelper<TModel> helper, 
            Expression<Func<TModel, TProperty>> expression,
            OptionsList<SelectListItem> options,
            int items = 10,
            bool allowClear = true,
            string placeHolder = null, 
            string optionLabel = null, 
            string containerClass = "select2_content", 
            string unconstrainedElement = null,
            string customFilterCallback = null,
            string dropdownCssClass = null)
        {
            return helper
                .SelectFor(expression, options)
                .AddDefaultOption(optionLabel)
                .ConfigSelect2(items, allowClear, placeHolder, containerClass, unconstrainedElement, customFilterCallback, dropdownCssClass);
        }

        public static HtmlTag Select2For<TModel, TValueProp, TTextProp>(
            this HtmlTagHelper<TModel> helper, 
            Expression<System.Func<TModel, TValueProp>> valueExpression, 
            Expression<System.Func<TModel, TTextProp>> textExpression, 
            string action, 
            string controller, 
            int items = 10, 
            bool allowClear = true, 
            string placeHolder = null, 
            bool disabled = false,
            string containerClass = "select2_content", 
            string unconstrainedElement = null, 
            string customFilterCallback = null)
        {
            //Get text from expression
            var text = string.Empty;
            try
            { text = textExpression.Compile().Invoke(helper.HtmlHelper.ViewData.Model).ToString(); }
            catch (Exception) 
            { /* Ignore */ }

            return helper.Select2For(
                valueExpression, text, action, controller, items, allowClear, 
                placeHolder, disabled, containerClass, unconstrainedElement, customFilterCallback);
        }

        public static HtmlTag Select2For<TModel, TValueProp>(
            this HtmlTagHelper<TModel> helper, 
            Expression<System.Func<TModel, TValueProp>> valueExpression, 
            string text, 
            string action, 
            string controller, 
            int items = 10, 
            bool allowClear = true, 
            string placeHolder = null, 
            bool disabled = false, 
            string containerClass = "select2_content", 
            string unconstrainedElement = null, 
            string customFilterCallback = null, 
            string dropdownCssClass = null)
        {
            if (disabled)
            {
                return helper.TextBox("-").Val(text).Disabled(true);
            }
            else
            {
                var name = Bardock.Utils.Linq.Expressions.ExpressionHelper.GetExpressionText(valueExpression);

                var hidden = helper
                    .HiddenFor(valueExpression)
                    .Data("dropdowncssclass", "drop_" + name)
                    .ConfigSelect2(items, allowClear, placeHolder, containerClass, unconstrainedElement, customFilterCallback, dropdownCssClass)
                    .Data("source-action", action)
                    .Data("source-controller", controller)
                    .Data("text", text);

                var fakeContainer = new HtmlTag("div")
                    .AddClasses("select2-container", "select2-fake")
                    .Append(new HtmlTag("a").AddClass("select2-choice"));

                return new NoTag().Append(fakeContainer).Append(hidden);
            }
        }

        private static HtmlTag ConfigSelect2(
            this HtmlTag tag,
            int items = 10,
            bool allowClear = true,
            string placeHolder = null,
            string containerClass = "select2_content",
            string unconstrainedElement = null,
            string customFilterCallback = null,
            string dropdownCssClass = null)
        {
            return tag
                .Data("provide", "select2")
                .Data("items", items)
                .Data("select2-container-class", containerClass)
                .Apply(t => t.Data("dropdowncssclass", string.Concat(t.Attr("data-dropdowncssclass"), " ", dropdownCssClass).Trim()),
                    when: !string.IsNullOrWhiteSpace(dropdownCssClass))
                .Apply(t => t.Data("placeholder", placeHolder),
                    when: !string.IsNullOrWhiteSpace(placeHolder))
                .Apply(t => t.Data("allow-clear", allowClear),
                    when: allowClear)
                .Apply(t => t.Data("unconstrained-element", unconstrainedElement),
                    when: !string.IsNullOrWhiteSpace(unconstrainedElement))
                .Apply(t => t.Data("custom-filter-callback", customFilterCallback),
                    when: !string.IsNullOrWhiteSpace(customFilterCallback));
        }

        public static HtmlTag Select2MultiFor<TModel, TValueProp>(
            this HtmlTagHelper<TModel> helper, 
            Expression<Func<TModel, TValueProp>> valueExpression, 
            IEnumerable<SelectListItem> items, 
            bool required = false)
        {
            return helper.SelectFor(valueExpression, OptionsList.Create(items))
                .BoolAttr("multiple", true)
                .Attr("data-provide", "select2");
        }

        public static MvcHtmlString FileUpload(
            this HtmlHelper htmlHelper, 
            string name, 
            string id = null, 
            string url = null)
        {
            return htmlHelper.Partial("~/Views/Shared/_FileUpload.cshtml", new Models.Shared.FileUpload
            {
                ID = id,
                Url = url,
                TextBoxTag = htmlHelper.Tags().File(name)
            });
        }

        public static MvcHtmlString FileUploadFor<TModel, TValue>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TValue>> expression, 
            string id = null, 
            string url = null, 
            string urlFieldName = "fileUrl", 
            string isEmptyFieldName = "fileIsEmpty")
        {
            return htmlHelper.Partial("~/Views/Shared/_FileUpload.cshtml", new Models.Shared.FileUpload
            {
                ID = id,
                Url = url,
                TextBoxTag = htmlHelper.Tags().FileFor(expression),
                UrlFieldName = urlFieldName,
                IsEmptyFieldName = isEmptyFieldName
            });
        }
    }
}