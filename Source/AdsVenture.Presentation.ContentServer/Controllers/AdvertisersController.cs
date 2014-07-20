using AdsVenture.Commons.Pagination;
using AdsVenture.Core.Managers;
using Bardock.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AdsVenture.Presentation.ContentServer.Controllers
{
    public class AdvertisersController : _BaseController
    {
        private readonly AdvertiserManager _manager;

        public AdvertisersController(AdvertiserManager mediagroupmanager)
        {
            this._manager = mediagroupmanager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index_Table(PageParams pageParams)
        {
            var page = _manager.FindAll(pageParams);
            return Json(new
            {
                Html = RenderViewToString("Index_Table", page.Data),
                TotalRecords = page.TotalRecords
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Add(Core.DTO.AdvertiserCreate data)
        {
            try
            {
                _manager.Create(data);
                Notifications.AddSuccess(Commons.Resources.Shared.Success_Add);
            }
            catch (Core.Exceptions.BusinessUserException ex)
            {
                Notifications.AddError(ex.Message);
            }
            catch (Core.Exceptions.BusinessException)
            {
                Notifications.AddError(Commons.Resources.Shared.Error_Undefined);
            }
            return RedirectToControllerHome();
        }

        [HttpPost]
        public ActionResult Edit(Core.DTO.AdvertiserUpdate data)
        {
            try
            {
                _manager.Update(data);
                Notifications.AddSuccess(Commons.Resources.Shared.Success_Update);
            }
            catch (Core.Exceptions.BusinessUserException ex)
            {
                Notifications.AddError(ex.Message);
            }
            catch (Core.Exceptions.BusinessException)
            {
                Notifications.AddError(Commons.Resources.Shared.Error_Undefined);
            }
            return RedirectToControllerHome();
        }
    }
}