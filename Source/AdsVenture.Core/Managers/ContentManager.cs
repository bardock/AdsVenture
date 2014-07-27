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
    public class ContentManager : _BaseEntityManager<Content>
    {
        public ContentManager(
            Helpers.IUnitOfWork unitOfWork)
            : base(unitOfWork) 
        {
        }

        private IQueryable<Content> GetActiveQuery(
            bool includeAdvertiser = false)
        {
            return Db.Contents
                .Where(x => x.Active)
                .Include(x => x.Advertiser, when: includeAdvertiser);
        }

        public virtual Content Find(Guid id)
        {
            return GetActiveQuery().FirstOrDefault(x => x.ID == id);
        }

        public virtual List<Content> FindAll(
            bool includeAdvertiser = false)
        {
            return GetActiveQuery(includeAdvertiser: includeAdvertiser).ToList();
        }

        public virtual List<Content> FindAll(Guid[] ids)
        {
            return GetActiveQuery()
                .Where(x => ids.Contains(x.ID))
                .ToList();
        }

        public virtual PageData<Content> FindAll(PageParams pageParams = null)
        {
            var query = GetActiveQuery(includeAdvertiser: true);

            if (pageParams != null && !string.IsNullOrEmpty(pageParams.Search))
            {
                query = query.Where(x =>
                    x.ID.ToString().Contains(pageParams.Search)
                    || x.Title.Contains(pageParams.Search)
                    || x.Advertiser.Name.Contains(pageParams.Search));
            }

            return query.Order(pageParams, x => x.ID).Page(pageParams);
        }

        public Content Create(DTO.ContentCreate data)
        {
            var e = Mapper.Map<Content>(data);
            e.ID = Guid.NewGuid();
            e.CreatedOn = DateTime.UtcNow;
            //e.CreatedByID = UserId;

            Db.Add(e).SaveChanges();
            return e;
        }

        public Content Update(DTO.ContentUpdate data)
        {
            var e = Find(data.ID);

            if (e == null)
                throw new Exceptions.EntityNotFoundException<Content>();

            e = Mapper.Map(data, e);
            e.UpdatedOn = DateTime.UtcNow;

            Db.Update(e).SaveChanges();
            return e;
        }

        public void Delete(Guid id)
        {
            var e = Find(id);
            Delete(e);
        }

        public void Delete(params Guid[] ids)
        {
            using (TransactionScope tx = GetTransactionScope())
            {
                foreach (var mg in FindAll(ids))
                {
                    Delete(mg);
                }
                tx.Complete();
            }
        }

        private void Delete(Content e)
        {
            e.Active = false;
            e.UpdatedOn = DateTime.UtcNow;

            Db.Update(e).SaveChanges();
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
