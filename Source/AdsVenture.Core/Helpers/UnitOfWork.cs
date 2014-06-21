using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AdsVenture.Data;

namespace AdsVenture.Core.Helpers
{
    public interface IUnitOfWork : IDisposable
    {
        DataContext GetDbContext();
    }

    public class UnitOfWork : IUnitOfWork
    {
        protected DataContext _dbContext;

        public UnitOfWork(Data.DataContext db)
        {
            _dbContext = db;
        }

        public DataContext GetDbContext()
        {
            return _dbContext;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}