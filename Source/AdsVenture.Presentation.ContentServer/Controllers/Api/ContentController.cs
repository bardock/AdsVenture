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
        public object RenderSlot(Guid slotID)
        {
            var c = _contentManager.Impress(slotID);
            return new
            {
                ContentID = c.ID,
                Html = _contentManager.Render(c)
            };
        }

        [HttpPost]
        [Route("slot/{slotID}/event")]
        public void PostSlotEvent(Guid slotID, Core.DTO.SlotEvent data)
        {
            data.SlotID = slotID;
            _contentManager.CreateSlotEvent(data);
        }
    }
}