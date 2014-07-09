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

namespace AdsVenture.Presentation.ContentServer.Helpers
{
    public static class SiteHtmlHelper
    {
        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, System.DateTime? value = null, string inputClass = null, bool appendCalendarMode = true, bool disabled = false)
        {
            return htmlHelper.Partial("~/Views/Shared/_DatePicker.cshtml", new Models.Shared.DatePicker
            {
                InputClass = inputClass,
                AppendCalendarMode = appendCalendarMode,
                Disabled = disabled,
                //Build a custom textbox to avoid date format
                //TODO: Create a custom textbox helper
                TextBoxBuilder = (IDictionary<string, object> htmlAttributes) => { 
                    return new MvcHtmlString(string.Format("<input type=\"text\" name=\"{0}\" value=\"{1}\" {2}>", 
                        name, value.ApplyOrDefault(x => x.DateFormat()), 
                        string.Join(" ", HtmlAttributeHelper.BuildAttrs(htmlAttributes, disabled: disabled)
                            .Select(x => string.Format("{0}=\"{1}\"", x.Key, x.Value))))); }
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
            if ((htmlHelper.ViewData.Model != null))
            {
                value = expression.Compile().Invoke(htmlHelper.ViewData.Model);
            }

            return htmlHelper.Partial("~/Views/Shared/_DatePicker.cshtml", new Models.Shared.DatePicker
            {
                InputClass = inputClass,
                AppendCalendarMode = appendCalendarMode,
                Disabled = disabled,
                MinViewMode = minViewMode,
                StartDate = startDate,
                EndDate = endDate,

                TextBoxBuilder = (IDictionary<string, object> htmlAttributes) =>
                {
                    htmlAttributes.Add("data-default-value", defaultValue.ApplyOrDefault(x => x.DateFormat()));

                    if ((expression.Body.NodeType == ExpressionType.Convert || expression.Body.NodeType == ExpressionType.ConvertChecked))
                    {
                        return htmlHelper.TextBox2For(
                            expression: (Expression<System.Func<TModel, System.DateTime>>)
                                Bardock.Utils.Linq.Expressions.ExpressionHelper.RemoveConvert(expression), 
                            htmlAttributes: htmlAttributes, 
                            value: value.ApplyOrDefault(x => x.DateFormat()), 
                            disabled: disabled);
                    }
                    else
                    {
                        return htmlHelper.TextBox2For(expression, htmlAttributes: htmlAttributes, value: value.ApplyOrDefault(x => x.DateFormat()), disabled: disabled);
                    }
                }
            });
        }

        public static MvcHtmlString Select2For<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            int items = 10,
            bool allowClear = true,
            string placeHolder = null, 
            string optionLabel = null, 
            IDictionary<string, object> htmlAttributes = null, 
            bool disabled = false,
            string containerClass = "select2_content", 
            string unconstrainedElement = null,
            string customFilterCallback = null,
            string dropdownCssClass = null)
        {
            htmlAttributes = GetSelect2HtmlAttributes(
                items, allowClear, placeHolder, htmlAttributes, containerClass, unconstrainedElement, customFilterCallback, dropdownCssClass);

            return htmlHelper.DropDownList2For(expression, selectList, optionLabel, htmlAttributes, disabled);
        }

        public static MvcHtmlString Select2For<TModel, TValueProp, TTextProp>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<System.Func<TModel, TValueProp>> valueExpression, 
            Expression<System.Func<TModel, TTextProp>> textExpression, 
            string action, 
            string controller, 
            int items = 10, 
            bool allowClear = true, 
            string placeHolder = null, 
            IDictionary<string, object> htmlAttributes = null, 
            bool disabled = false,
            string containerClass = "select2_content", 
            string unconstrainedElement = null, 
            string customFilterCallback = null)
        {
            //Get text from expression
            var text = string.Empty;
            try
            {
                text = textExpression.Compile().Invoke(htmlHelper.ViewData.Model).ToString();
            }
            catch (Exception)
            {
                //Ignore
            }
            return htmlHelper.Select2For(valueExpression, text, action, controller, items, allowClear, placeHolder, htmlAttributes, disabled, containerClass,
            unconstrainedElement, customFilterCallback);
        }

        public static MvcHtmlString Select2For<TModel, TValueProp>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<System.Func<TModel, TValueProp>> valueExpression, 
            string text, 
            string action, 
            string controller, 
            int items = 10, 
            bool allowClear = true, 
            string placeHolder = null, 
            IDictionary<string, object> htmlAttributes = null, 
            bool disabled = false, 
            string containerClass = "select2_content", 
            string unconstrainedElement = null, 
            string customFilterCallback = null, 
            string dropdownCssClass = null)
        {
            if (disabled)
            {
                return htmlHelper.TextBox2("-", text, disabled: true);

            }
            else
            {
                var name = Bardock.Utils.Linq.Expressions.ExpressionHelper.GetExpressionText(valueExpression);

                htmlAttributes["data-dropdowncssclass"] = "drop_" + name;
                htmlAttributes = GetSelect2HtmlAttributes(
                    items, allowClear, placeHolder, htmlAttributes, containerClass, unconstrainedElement, customFilterCallback, dropdownCssClass);
                htmlAttributes["data-source-action"] = action;
                htmlAttributes["data-source-controller"] = controller;
                htmlAttributes["data-text"] = text;

                var hidden = htmlHelper.HiddenFor(valueExpression, htmlAttributes);
                var fakeContainer = "<div class=\"select2-container select2-fake\"><a class=\"select2-choice\"></a></div>";

                var htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine(fakeContainer).AppendLine(hidden.ToString());
                return new MvcHtmlString(htmlBuilder.ToString());
            }
        }

        private static IDictionary<string, object> GetSelect2HtmlAttributes(
            int items = 10,
            bool allowClear = true,
            string placeHolder = null,
            IDictionary<string, object> htmlAttributes = null,
            string containerClass = "select2_content",
            string unconstrainedElement = null,
            string customFilterCallback = null,
            string dropdownCssClass = null)
        {
            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();

            htmlAttributes["data-provide"] = "select2";
            htmlAttributes["data-items"] = items;
            htmlAttributes["data-select2-container-class"] = containerClass;
            if (!string.IsNullOrEmpty(dropdownCssClass))
            {
                htmlAttributes["data-dropdowncssclass"] = 
                    htmlAttributes.ContainsKey("data-dropdowncssclass") 
                    ? string.Concat(htmlAttributes["data-dropdowncssclass"], " ", dropdownCssClass) 
                    : dropdownCssClass;
            }

            if (!string.IsNullOrWhiteSpace(placeHolder))
            {
                htmlAttributes["data-placeholder"] = placeHolder;
            }

            if (allowClear)
            {
                htmlAttributes["data-allow-clear"] = allowClear;
            }

            if (!string.IsNullOrWhiteSpace(unconstrainedElement))
            {
                htmlAttributes["data-unconstrained-element"] = unconstrainedElement;
            }

            if (!string.IsNullOrWhiteSpace(customFilterCallback))
            {
                htmlAttributes["data-custom-filter-callback"] = customFilterCallback;
            }

            return htmlAttributes;
        }

        public static MvcHtmlString Select2MultiFor<TModel, TValueProp>(this HtmlHelper<TModel> htmlHelper, Expression<System.Func<TModel, TValueProp>> valueExpression, IEnumerable<SelectListItem> items, IDictionary<string, object> htmlAttributes = null, bool disabled = false, bool required = false)
        {
            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();
            htmlAttributes["data-provide"] = "select2";
            if ((disabled))
            {
                htmlAttributes["disabled"] = "disabled";
            }
            if ((required))
            {
                htmlAttributes["data-val"] = "true";
                htmlAttributes["data-val-required"] = "";
            }
            return htmlHelper.ListBoxFor(valueExpression, items, htmlAttributes);
        }

        public static MvcHtmlString FileUpload(this HtmlHelper htmlHelper, string name, string id = null, string url = null)
        {
            return htmlHelper.Partial("~/Views/Shared/_FileUpload.cshtml", new Models.Shared.FileUpload
            {
                ID = id,
                Url = url,
                TextBoxBuilder = (IDictionary<string, object> htmlAttributes) => { return htmlHelper.TextBox(name, null, new { Type = "file" }); }
            });
        }

        public static MvcHtmlString FileUploadFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string id = null, string url = null, string urlFieldName = "fileUrl", string isEmptyFieldName = "fileIsEmpty")
        {
            return htmlHelper.Partial("~/Views/Shared/_FileUpload.cshtml", new Models.Shared.FileUpload
            {
                ID = id,
                Url = url,
                TextBoxBuilder = (IDictionary<string, object> htmlAttributes) =>
                {
                    htmlAttributes.Add("type", "file");
                    return htmlHelper.TextBox2For(expression, htmlAttributes: htmlAttributes);
                },
                UrlFieldName = urlFieldName,
                IsEmptyFieldName = isEmptyFieldName
            });
        }
    }
}