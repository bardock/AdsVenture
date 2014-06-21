using AdsVenture.Commons.Entities;
using AdsVenture.Commons.Entities.Interfaces;
using AdsVenture.Core.Extensions;
using Bardock.Utils.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AdsVenture.Core.Managers
{
    public abstract class _EventLogManager<TEventLog, TEntity> : _BaseEntityManager<TEventLog>
        where TEventLog : class, IEventLog
        where TEntity : class, IEntity
    {

        protected abstract IEnumerable<string> IgnoredProperties
        {
            get;
        }

        protected abstract IDbSet<TEventLog> EntitySet { get; }

        public _EventLogManager(Helpers.IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public virtual TEventLog Find(Int32 id)
        {
            return EntitySet.Find(id);
        }

        public virtual List<TEventLog> FindAll()
        {
            return EntitySet.ToList();
        }

        protected abstract TEventLog Create();

        protected virtual TEventLog Create(int userId, TEntity entity)
        {
            var e = Create();
            e.Entity = entity;
            e.EventDate = DateTime.UtcNow;
            return e;
        }

        internal virtual void CreateLog(TEntity entity)
	    {
		    foreach (var changedField in GetChangedFields(entity)) {
			    var log = Create(UserId, entity);
			    log.FieldName = changedField.Key;
			    log.FieldValueBefore = changedField.Value[0] == null ? null : changedField.Value[0].ToString();
                log.FieldValueAfter = changedField.Value[1] == null ? null : changedField.Value[1].ToString();
			    Db.Add(log);
		    }
	    }

        /// <summary>
        /// Returns a dictionary with changed fields.
        /// Each field has field name as key and an array with field values (before and after) as value
        /// </summary>
        /// <param name="entity">Uses DbEntityEntry.OriginalValues and current instance values</param>
        protected Dictionary<string, object[]> GetChangedFields(TEntity entity)
	    {
            return Db.Entry(entity).GetChangedFields(entity, IgnoredProperties);
	    }

    }
}
