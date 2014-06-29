using AdsVenture.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AdsVenture.Presentation.ContentServer.Controllers.Api
{
    [RoutePrefix("api/content")]
    public class ContentController : ApiController
    {
        ContentManager _contentManager;

        public ContentController(ContentManager contentManager)
        {
            this._contentManager = contentManager;
        }

        [HttpGet]
        [Route("slot/{slotID}")]
        public string RenderSlot(Guid slotID)
        {
            var c = _contentManager.Impress(slotID);
            return _contentManager.Render(c);
        }
    }
}