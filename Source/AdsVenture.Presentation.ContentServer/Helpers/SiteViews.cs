﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Runtime.CompilerServices;
using Bardock.Utils.Web.Mvc.Extensions;
using Bardock.Utils.Web.Mvc.Helpers;

namespace AdsVenture.Presentation.ContentServer.Helpers
{
    public static class SiteViews
    {
        public static System.Web.Mvc.MvcHtmlString Partial_ConfirmModal(
            this System.Web.Mvc.HtmlHelper htmlHelper, 
            Models.Shared.ConfirmModal model)
        {
            return htmlHelper.Partial("~/Views/Shared/_ConfirmModal.cshtml", model);
        }
    }
}