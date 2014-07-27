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
    public class CampaignManager : _BaseEntityManager<Campaign>
    {
        public CampaignManager(
            Helpers.IUnitOfWork unitOfWork)
            : base(unitOfWork) 
        {
        }

        internal IQueryable<Campaign> GetActiveQuery()
        {
            return Db.Campaigns
                .Where(x => x.Active);
        }

        internal IQueryable<Campaign> GetToConsumeQuery()
        {
            return GetActiveQuery()
                .Where(x => x.EndsOn == null || x.EndsOn > DateTime.UtcNow);
        }

        public virtual Campaign Find(Guid id)
        {
            return GetActiveQuery().FirstOrDefault(x => x.ID == id);
        }

        public virtual List<Campaign> FindAll()
        {
            return GetActiveQuery().ToList();
        }

        public virtual List<Campaign> FindAll(Guid[] ids)
        {
            return GetActiveQuery()
                .Where(x => ids.Contains(x.ID))
                .ToList();
        }

        public virtual PageData<Campaign> FindAll(PageParams pageParams = null)
        {
            var query = GetActiveQuery()
                .Include(x => x.Advertiser);

            if (pageParams != null && !string.IsNullOrEmpty(pageParams.Search))
            {
                query = query.Where(x => 
                    x.ID.ToString().Contains(pageParams.Search)
                    || x.Title.Contains(pageParams.Search)
                    || x.Advertiser.Name.Contains(pageParams.Search));
            }

            return query.Order(pageParams, x => x.ID).Page(pageParams);
        }

        public Campaign Create(DTO.CampaignCreate data)
        {
            var e = Mapper.Map<Campaign>(data);
            e.ID = Guid.NewGuid();
            e.CreatedOn = DateTime.UtcNow;
            //e.CreatedByID = UserId;

            Db.Add(e).SaveChanges();
            return e;
        }

        public Campaign Update(DTO.CampaignUpdate data)
        {
            var e = Find(data.ID);

            if (e == null)
                throw new Exceptions.EntityNotFoundException<Campaign>();

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

        private void Delete(Campaign e)
        {
            e.Active = false;
            e.UpdatedOn = DateTime.UtcNow;

            Db.Update(e).SaveChanges();
        }
    }
}
