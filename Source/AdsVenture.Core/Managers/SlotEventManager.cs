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
    public class SlotEventManager : _BaseEntityManager<SlotEvent>
    {
        public SlotEventManager(
            Helpers.IUnitOfWork unitOfWork)
            : base(unitOfWork) 
        {
        }

        private IQueryable<SlotEvent> GetQuery(
            bool includeContent = false)
        {
            return Db.SlotEvents
                .Include(x => x.Content, when: includeContent);
        }

        public virtual SlotEvent Find(Guid id)
        {
            return GetQuery().FirstOrDefault(x => x.ID == id);
        }

        public virtual List<SlotEvent> FindAll()
        {
            return GetQuery().ToList();
        }

        public virtual List<SlotEvent> FindAllByCampaign(Guid campaignID)
        {
            return GetQuery()
                .Where(x => x.CampaignID == campaignID)
                .ToList();
        }

        internal void CreateImpression(Guid contentID, Guid campaignID, Guid slotID)
        {
            var e = new SlotEvent() 
            {
                ID = Guid.NewGuid(),
                Discriminator = SlotEventDiscriminator.Impression,
                ContentID = contentID,
                CampaignID = campaignID,
                SlotID = slotID,
                Date = DateTime.UtcNow
            };
            Db.Add(e).SaveChanges();
        }

        internal void CreateUserInteraction(Guid? campaignID, DTO.SlotUserEvent data)
        {
            var e = Mapper.Map<SlotEvent>(data);
            e.ID = Guid.NewGuid();
            e.Discriminator = SlotEventDiscriminator.UserInteraction;
            e.CampaignID = campaignID;
            Db.Add(e).SaveChanges();
        }
    }
}
