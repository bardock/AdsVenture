using AdsVenture.Commons.Entities;
using AdsVenture.Commons.Pagination;
using AdsVenture.Commons.Pagination.Extensions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace AdsVenture.Core.Managers
{
    public class AdvertiserManager : _BaseEntityManager<Content>
    {
        public AdvertiserManager(
            Helpers.IUnitOfWork unitOfWork)
            : base(unitOfWork) 
        {
        }

        private IQueryable<Advertiser> GetActiveQuery()
        {
            return Db.Advertisers.Where(x => x.Active);
        }

        public virtual Advertiser Find(Guid id)
        {
            return GetActiveQuery().FirstOrDefault(x => x.ID == id);
        }

        public virtual List<Advertiser> FindAll()
        {
            return GetActiveQuery().ToList();
        }

        public virtual List<Advertiser> FindAll(Guid[] ids)
        {
            return GetActiveQuery()
                .Where(x => ids.Contains(x.ID))
                .ToList();
        }

        public virtual PageData<Advertiser> FindAll(PageParams pageParams = null)
        {
            var query = GetActiveQuery();

            if (pageParams != null && !string.IsNullOrEmpty(pageParams.Search))
            {
                query = query.Where(p => p.Name.Contains(pageParams.Search));
            }

            return query.Order(pageParams, x => x.ID).Page(pageParams);
        }

        private void ValidateCreate(DTO.AdvertiserCreate data)
        {
            var name = data.Name.ToLower().Trim();

            var alreadyExistName = Db.Advertisers.Any(x => x.Name.ToLower() == name);

            if (alreadyExistName)
                throw new InvalidMediaGroupDescriptionException(data.Name);
        }

        public class InvalidMediaGroupDescriptionException : Exceptions.BusinessUserException
        {
            public InvalidMediaGroupDescriptionException(string name)
                : base(String.Format(Resources.BusinessExceptions.Advertiser_Name_AlreadyExists, name)) { }
        }

        public Advertiser Create(DTO.AdvertiserCreate data)
        {
            ValidateCreate(data);

            var e = Mapper.Map<Advertiser>(data);
            e.ID = Guid.NewGuid();
            e.CreatedOn = DateTime.UtcNow;
            //e.CreatedByID = UserId;

            Db.Add(e).SaveChanges();
            return e;
        }

        public Advertiser Update(DTO.AdvertiserUpdate data)
        {
            var e = Find(data.ID);

            if (e == null)
                throw new Exceptions.EntityNotFoundException<Advertiser>();

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

        private void Delete(Advertiser e)
        {
            e.Active = false;
            e.UpdatedOn = DateTime.UtcNow;

            Db.Update(e).SaveChanges();
        }
    }
}
