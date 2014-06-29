using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Bardock.Utils.Extensions;
using AdsVenture.Commons.Entities;

namespace AdsVenture.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbConnection connection)
            : base(connection, true) { }

        public DataContext()
            : base("DataContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Properties<string>().Configure(p => p.IsUnicode(false));
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        #region DbSets

        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<ContentImpression> ContentImpressions { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Slot> Slots { get; set; }

        #endregion

        #region Helpers

        public DataContext Delete(Commons.Entities.Interfaces.IEntity e)
        {
            Entry(e).State = EntityState.Deleted;
            return this;
        }

        public DataContext Delete<T>(IEnumerable<T> e) where T : class, Commons.Entities.Interfaces.IEntity
        {
            foreach (var item in e)
            {
                Delete(item);
            }
            return this;
        }

        public DataContext Add(Commons.Entities.Interfaces.IEntity e)
        {
            if ((Entry(e).State != EntityState.Modified))
            {
                Entry(e).State = EntityState.Added;
            }
            return this;
        }

        public DataContext Add<T>(IEnumerable<T> e) where T : class, Commons.Entities.Interfaces.IEntity
        {
            foreach (var item in e)
            {
                Add(item);
            }
            return this;
        }

        public DataContext Update(Commons.Entities.Interfaces.IEntity e)
        {
            if ((Entry(e).State != EntityState.Added))
            {
                Entry(e).State = EntityState.Modified;
            }
            return this;
        }

        public DataContext Update<T>(IEnumerable<T> e) where T : class, Commons.Entities.Interfaces.IEntity
        {
            foreach (var item in e)
            {
                Update(item);
            }
            return this;
        }

        public DataContext Detach(Commons.Entities.Interfaces.IEntity e)
        {
            Entry(e).State = EntityState.Detached;
            return this;
        }

        public DataContext Detach<T>(IEnumerable<T> e) where T : class, Commons.Entities.Interfaces.IEntity
        {
            foreach (var item in e)
            {
                Detach(item);
            }
            return this;
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw GetSaveChangesException(ex);
            }
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            try
            {
                return await base.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw GetSaveChangesException(ex);
            }
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                throw GetSaveChangesException(ex);
            }
        }

        private Exception GetSaveChangesException(DbUpdateException ex)
        {
            var sqlEx = GetInnerSqlExceptionOrDefault(ex);
            if (sqlEx != null && sqlEx.Number.In(2601, 2627))
            {
                // 2601: http://technet.microsoft.com/en-us/library/ms151779(v=sql.105).aspx
                // 2627: http://technet.microsoft.com/en-us/library/ms151757(v=sql.105).aspx
                return new Exceptions.DuplicatedEntryException(ex);
            }
            return ex;
        }

        private SqlException GetInnerSqlExceptionOrDefault(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                if (typeof(SqlException).IsAssignableFrom(ex.GetType()))
                    return (SqlException)ex;
            }
            return null;
        }

        #endregion
    }
}