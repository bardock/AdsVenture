using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bardock.Utils.Web;

namespace AdsVenture.Presentation.ContentServer.Helpers.Extensions
{
    public static class NotificationsExtensions
    {
        //TODO
        //public static System.Web.Mvc.MvcHtmlString Notifications(this System.Web.Mvc.HtmlHelper htmlHelper)
        //{
        //    return htmlHelper.Action("_List", "Notifications");
        //}

        public static IEnumerable<object> ToUIModel(this IEnumerable<RequestNotifications.Item> items)
        {
            return items.Select(x => new
            {
                level = x.Type == RequestNotifications.NotificationType.ERR ? "danger" : x.Type.ToString().ToLower(),
                data = x.Message
            }).ToList();
        }

    }
}