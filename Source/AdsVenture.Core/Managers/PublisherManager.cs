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
    public class PublisherManager : _BaseEntityManager<Content>
    {
        public PublisherManager(
            Helpers.IUnitOfWork unitOfWork)
            : base(unitOfWork) 
        {
        }

        private IQueryable<Publisher> GetActiveQuery()
        {
            return Db.Publishers.Where(x => x.Active);
        }

        public virtual Publisher Find(Guid id)
        {
            return GetActiveQuery().FirstOrDefault(x => x.ID == id);
        }

        public virtual List<Publisher> FindAll()
        {
            return GetActiveQuery().ToList();
        }

        public virtual List<Publisher> FindAll(Guid[] ids)
        {
            return GetActiveQuery()
                .Where(x => ids.Contains(x.ID))
                .ToList();
        }

        public virtual PageData<Publisher> FindAll(PageParams pageParams = null)
        {
            var query = GetActiveQuery();

            if (pageParams != null && !string.IsNullOrEmpty(pageParams.Search))
            {
                query = query.Where(x => 
                    x.ID.ToString().Contains(pageParams.Search)
                    || x.Name.Contains(pageParams.Search));
            }

            return query.Order(pageParams, x => x.ID).Page(pageParams);
        }

        private void ValidateCreate(DTO.PublisherCreate data)
        {
            var name = data.Name.ToLower().Trim();

            var alreadyExistName = Db.Publishers.Any(x => x.Active && x.Name.ToLower() == name);

            if (alreadyExistName)
                throw new InvalidMediaGroupDescriptionException(data.Name);
        }

        public class InvalidMediaGroupDescriptionException : Exceptions.BusinessUserException
        {
            public InvalidMediaGroupDescriptionException(string name)
                : base(String.Format(Resources.BusinessExceptions.Publisher_Name_AlreadyExists, name)) { }
        }

        public Publisher Create(DTO.PublisherCreate data)
        {
            ValidateCreate(data);

            var e = Mapper.Map<Publisher>(data);
            e.ID = Guid.NewGuid();
            e.CreatedOn = DateTime.UtcNow;
            //e.CreatedByID = UserId;

            Db.Add(e).SaveChanges();
            return e;
        }

        public Publisher Update(DTO.PublisherUpdate data)
        {
            var e = Find(data.ID);

            if (e == null)
                throw new Exceptions.EntityNotFoundException<Publisher>();

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

        private void Delete(Publisher e)
        {
            e.Active = false;
            e.UpdatedOn = DateTime.UtcNow;

            Db.Update(e).SaveChanges();
        }
    }
}
