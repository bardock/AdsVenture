using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdsVenture.Commons.Entities;
using AdsVenture.Core.Managers;
using AdsVenture.Core;
using AdsVenture.Commons.Pagination;
using Bardock.Utils.Web.Mvc.Helpers;
using Bardock.Utils.Extensions;

namespace AdsVenture.Presentation.ContentServer.Controllers
{
    public class AdvertisersController : _BaseController
    {
        //private readonly AdvertiserManager _mediaGroupManager;

        //public AdvertisersController(AdvertiserManager mediagroupmanager)
        //{
        //    this._mediaGroupManager = mediagroupmanager;
        //}

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Index_Table(PageParams pageParams)
        //{
        //    var page = _mediaGroupManager.FindAll(pageParams);
        //    return Json(new
        //    {
        //        Html = RenderViewToString("Index_Table", page.Data),
        //        TotalRecords = page.TotalRecords
        //    }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult Add(Core.DTO.Advertiser data)
        //{
        //    try
        //    {
        //        _mediaGroupManager.Create(data);
        //        Notifications.AddSuccess(Commons.Resources.Shared.Success_Add);
        //    }
        //    catch (Core.Exceptions.BusinessUserException ex)
        //    {
        //        Notifications.AddError(ex.Message);
        //    }
        //    catch (Core.Exceptions.BusinessException)
        //    {
        //        Notifications.AddError(Commons.Resources.Shared.Error_Undefined);
        //    }
        //    return RedirectToControllerHome();
        //}

        //[HttpPost]
        //public ActionResult Edit(Core.DTO.AdvertiserUpdate data)
        //{
        //    try
        //    {
        //        _mediaGroupManager.Update(data);
        //        Notifications.AddSuccess(Commons.Resources.Shared.Success_Update);
        //    }
        //    catch (Core.Exceptions.BusinessUserException ex)
        //    {
        //        Notifications.AddError(ex.Message);
        //    }
        //    catch (Core.Exceptions.BusinessException)
        //    {
        //        Notifications.AddError(Commons.Resources.Shared.Error_Undefined);
        //    }
        //    return RedirectToControllerHome();
        //}

    }
}