using AdsVenture.Commons.Pagination;
using AdsVenture.Core.Managers;
using AutoMapper;
using Bardock.Utils.Extensions;
using Bardock.Utils.Web.Mvc.HtmlTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AdsVenture.Presentation.ContentServer.Controllers
{
    public class ContentsController : _BaseController
    {
        private readonly ContentManager _manager;
        private readonly AdvertiserManager _advertiserManager;

        public ContentsController(
            ContentManager mediagroupmanager,
            AdvertiserManager advertiserManager)
        {
            this._manager = mediagroupmanager;
            this._advertiserManager = advertiserManager;
        }

        public ActionResult Index()
        {
            ViewBag.AddPathNodeWithTitle = Resources.Global.Search;
            return View();
        }

        public ActionResult Index_Table(PageParams pageParams)
        {
            return TablePage(_manager.FindAll(pageParams));
        }

        public ActionResult Add()
        {
            return GetAddView();
        }

        private ActionResult GetAddView(Commons.Entities.Content e = null)
        {
            return GetFormView(e, isNew: true);
        }

        [HttpPost]
        public ActionResult Add(Core.DTO.ContentCreate data)
        {
            try
            {
                _manager.Create(data);
                Notifications.AddSuccess(Commons.Resources.Shared.Success_Add);
                return RedirectToControllerHome();
            }
            catch (Core.Exceptions.BusinessUserException ex)
            {
                Notifications.AddError(ex.Message);
            }
            catch (Core.Exceptions.BusinessException)
            {
                Notifications.AddError(Commons.Resources.Shared.Error_Undefined);
            }
            return GetAddView(Mapper.Map<Commons.Entities.Content>(data));
        }

        public ActionResult Edit(Guid id)
        {
            return GetEditView(id);
        }

        private ActionResult GetEditView(Guid id, Core.DTO.ContentUpdate data = null)
        {
            var e = _manager.Find(id);

            if (e == null)
            {
                ThrowHttpNotFound();
            }
            
            ViewBag.AddPathNodeWithTitle = e.Title;

            if(data != null)
            { 
                e = Mapper.Map(data, e);
            }
            return GetFormView(e);
        }

        [HttpPost]
        public ActionResult Edit(Core.DTO.ContentUpdate data)
        {
            try
            {
                _manager.Update(data);
                Notifications.AddSuccess(Commons.Resources.Shared.Success_Update);
                return RedirectToControllerHome();
            }
            catch (Core.Exceptions.BusinessUserException ex)
            {
                Notifications.AddError(ex.Message);
            }
            catch (Core.Exceptions.BusinessException)
            {
                Notifications.AddError(Commons.Resources.Shared.Error_Undefined);
            }
            return GetEditView(data.ID, data);
        }

        private ActionResult GetFormView(Commons.Entities.Content e = null, bool isNew = false)
        {
            var model = new Models.Contents.Form()
            {
                IsNew = isNew,
                AdvertiserOptions = OptionsList.Create(_advertiserManager.FindAll(), display: x => x.Name, value: x => x.ID),
                Entity = e
            };
            return View("Form", model);
        }
    }
}