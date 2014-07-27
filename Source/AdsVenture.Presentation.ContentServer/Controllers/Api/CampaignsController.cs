using AdsVenture.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AdsVenture.Presentation.ContentServer.Controllers.Api
{
    public class CampaignsController : ApiController
    {
        CampaignManager _manager;

        public CampaignsController(CampaignManager contentManager)
        {
            this._manager = contentManager;
        }

        [HttpDelete]
        public void DeleteMassive(Models.Shared.MassiveParams<Guid> param)
        {
            _manager.Delete(param.IDs);
        }
    }
}