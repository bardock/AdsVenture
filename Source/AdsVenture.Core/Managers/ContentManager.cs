using AdsVenture.Commons.Entities;
using AdsVenture.Core.Exceptions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AdsVenture.Core.Managers
{
    public class ContentManager : _BaseEntityManager<Content>
    {
        public ContentManager(
            Helpers.IUnitOfWork unitOfWork)
            : base(unitOfWork) 
        {
        }

        public Content Impress(Guid slotID)
        {
            var slot = Db.Slots
                .AsNoTracking()
                .Include(x => x.Content)
                .FirstOrDefault(x => x.ID == slotID);

            if (slot.ContentID == null)
                throw new NotImplementedException("Slot has not a content");

            var content = slot.Content;
            var advertiserID = content.AdvertiserID;

            var campaign = Db.Campaigns
                .AsNoTracking()
                .Where(x => x.AdvertiserID == advertiserID)
                .Where(x => x.Active)
                .Where(x => x.EndsOn == null || x.EndsOn > DateTime.UtcNow)
                .FirstOrDefault();

            if (campaign == null)
                throw new Exceptions.BusinessException(string.Format("There is no active campaign for advertiser '{0}'", advertiserID.ToString("n")));

            var impression = new ContentImpression()
            {
                ContentID = content.ID,
                CampaignID = campaign.ID,
                SlotID = slot.ID,
                CreatedOn = DateTime.UtcNow
            };
            Db.Add(impression).SaveChanges();

            return content;
        }

        public string Render(Content e)
        {
            return string.Format("<iframe src=\"{0}\"></iframe>", e.Url);
        }

        public void CreateSlotEvent(DTO.SlotEvent data)
        {
            var e = Mapper.Map<SlotEvent>(data);
            e.ID = Guid.NewGuid();
            Db.Add(e).SaveChanges();
        }
    }
}
