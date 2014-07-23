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
    public class PublishersController : _BaseController
    {
        private readonly PublisherManager _manager;
        private readonly CountryManager _countryManager;

        public PublishersController(
            PublisherManager mediagroupmanager, 
            CountryManager countryManager)
        {
            this._manager = mediagroupmanager;
            this._countryManager = countryManager;
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

        private ActionResult GetAddView(Commons.Entities.Publisher e = null)
        {
            return GetFormView(e, isNew: true);
        }

        [HttpPost]
        public ActionResult Add(Core.DTO.PublisherCreate data)
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
            return GetAddView(Mapper.Map<Commons.Entities.Publisher>(data));
        }

        public ActionResult Edit(Guid id)
        {
            return GetEditView(id);
        }

        private ActionResult GetEditView(Guid id, Core.DTO.PublisherUpdate data = null)
        {
            var e = _manager.Find(id);

            if (e == null)
            {
                ThrowHttpNotFound();
            }
            
            ViewBag.AddPathNodeWithTitle = e.Name;

            if(data != null)
            { 
                e = Mapper.Map(data, e);
            }
            return GetFormView(e);
        }

        [HttpPost]
        public ActionResult Edit(Core.DTO.PublisherUpdate data)
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

        private ActionResult GetFormView(Commons.Entities.Publisher e = null, bool isNew = false)
        {
            var model = new Models.Publishers.Form()
            {
                IsNew = isNew,
                CountryOptions = OptionsList.Create(_countryManager.FindAll(), display: x => x.Description, value: x => x.ID),
                Entity = e
            };
            return View("Form", model);
        }
    }
}