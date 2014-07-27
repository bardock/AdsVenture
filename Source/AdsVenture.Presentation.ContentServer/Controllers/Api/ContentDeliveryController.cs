using AdsVenture.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AdsVenture.Presentation.ContentServer.Controllers.Api
{
    [RoutePrefix("api/contentDelivery")]
    public class ContentDeliveryController : ApiController
    {
        ContentDeliveryManager _manager;

        public ContentDeliveryController(ContentDeliveryManager manager)
        {
            this._manager = manager;
        }

        [HttpGet]
        [Route("slot/{slotID}")]
        public object RenderSlot(Guid slotID)
        {
            var c = _manager.Impress(slotID);
            return new
            {
                ContentID = c.ID,
                Html = _manager.Render(c)
            };
        }

        [HttpPost]
        [Route("slot/{slotID}/event")]
        public void PostSlotEvent(Guid slotID, Core.DTO.SlotUserEvent data)
        {
            data.SlotID = slotID;
            _manager.CreateSlotUserEvent(data);
        }
    }
}