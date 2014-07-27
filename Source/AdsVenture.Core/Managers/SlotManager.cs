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
    public class SlotManager : _BaseEntityManager<Slot>
    {
        public SlotManager(
            Helpers.IUnitOfWork unitOfWork)
            : base(unitOfWork) 
        {
        }

        internal IQueryable<Slot> GetActiveQuery()
        {
            return Db.Slots
                .Where(x => x.Active);
        }

        public virtual Slot Find(Guid id)
        {
            return GetActiveQuery().FirstOrDefault(x => x.ID == id);
        }

        public virtual List<Slot> FindAll()
        {
            return GetActiveQuery().ToList();
        }

        public virtual List<Slot> FindAll(Guid[] ids)
        {
            return GetActiveQuery()
                .Where(x => ids.Contains(x.ID))
                .ToList();
        }

        public virtual PageData<Slot> FindAll(PageParams pageParams = null)
        {
            var query = GetActiveQuery()
                .Include(x => x.Publisher);

            if (pageParams != null && !string.IsNullOrEmpty(pageParams.Search))
            {
                query = query.Where(x => 
                    x.ID.ToString().Contains(pageParams.Search)
                    || x.Title.Contains(pageParams.Search)
                    || x.Publisher.Name.Contains(pageParams.Search));
            }

            return query.Order(pageParams, x => x.ID).Page(pageParams);
        }

        public Slot Create(DTO.SlotCreate data)
        {
            var e = Mapper.Map<Slot>(data);
            e.ID = Guid.NewGuid();
            e.CreatedOn = DateTime.UtcNow;
            //e.CreatedByID = UserId;

            Db.Add(e).SaveChanges();
            return e;
        }

        public Slot Update(DTO.SlotUpdate data)
        {
            var e = Find(data.ID);

            if (e == null)
                throw new Exceptions.EntityNotFoundException<Slot>();

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

        private void Delete(Slot e)
        {
            e.Active = false;
            e.UpdatedOn = DateTime.UtcNow;

            Db.Update(e).SaveChanges();
        }
    }
}
