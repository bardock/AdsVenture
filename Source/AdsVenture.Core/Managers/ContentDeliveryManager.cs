using AdsVenture.Commons.Entities;
using AdsVenture.Commons.Pagination;
using AdsVenture.Commons.Pagination.Extensions;
using AdsVenture.Core.Exceptions;
using AdsVenture.Data.Helpers.Extensions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace AdsVenture.Core.Managers
{
    public class ContentDeliveryManager : _BaseManager
    {
        private ContentManager _contentManager;
        private CampaignManager _campaignManager;
        private SlotManager _slotManager;
        private SlotEventManager _slotEventManager;

        public ContentDeliveryManager(
            Helpers.IUnitOfWork unitOfWork,
            ContentManager contentManager,
            CampaignManager campaignManager,
            SlotManager slotManager,
            SlotEventManager slotEventManager)
            : base(unitOfWork) 
        {
            this._contentManager = contentManager;
            this._campaignManager = campaignManager;
            this._slotManager = slotManager;
            this._slotEventManager = slotEventManager;
        }

        public Content Impress(Guid slotID)
        {
            var slot = _slotManager.GetActiveQuery()
                .AsNoTracking()
                .Include(x => x.Content)
                .FirstOrDefault(x => x.ID == slotID);

            var content = ResolveContentToImpress(slot); ;
            var advertiserID = content.AdvertiserID;

            var campaign = _campaignManager.GetToConsumeQuery()
                   .AsNoTracking()
                   .Where(x => x.AdvertiserID == advertiserID)
                   .FirstOrDefault();

            if (campaign == null)
                throw new Exceptions.BusinessException(string.Format("There is no active campaign for advertiser '{0}'", advertiserID.ToString("n")));

            _slotEventManager.CreateImpression(content.ID, campaign.ID, slot.ID);

            return content;
        }

        private Content ResolveContentToImpress(Slot slot)
        {
            if (slot.ContentID == null)
                throw new NotImplementedException("Slot has not a content");

            return slot.Content;
        }

        public string Render(Content e)
        {
            return string.Format("<iframe src=\"{0}\"></iframe>", e.Url);
        }

        public void CreateSlotUserEvent(DTO.SlotUserEvent data)
        {
            var campaignID = _campaignManager.GetToConsumeQuery()
                   .Where(x => x.Advertiser.Contents.Any(c => c.ID == data.ContentID))
                   .Select(x => (Guid?)x.ID)
                   .FirstOrDefault();
            _slotEventManager.CreateUserInteraction(campaignID, data);
        }
    }
}
