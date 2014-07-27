using AdsVenture.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AdsVenture.Presentation.ContentServer.Controllers.Api
{
    [RoutePrefix("api/contents")]
    public class ContentsController : ApiController
    {
        ContentManager _manager;

        public ContentsController(ContentManager contentManager)
        {
            this._manager = contentManager;
        }

        [HttpDelete]
        public void DeleteMassive(Models.Shared.MassiveParams<Guid> param)
        {
            _manager.Delete(param.IDs);
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
        public void PostSlotEvent(Guid slotID, Core.DTO.SlotEvent data)
        {
            data.SlotID = slotID;
            _manager.CreateSlotEvent(data);
        }
    }
}