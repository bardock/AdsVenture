using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdsVenture.Data;
using AdsVenture.Commons;
using System.Transactions;

namespace AdsVenture.Core.Managers
{
    public abstract class _BaseManager : IManager
    {
        protected Helpers.IUnitOfWork _unitOfWork;

        protected int _userId = 0;
        public _BaseManager(Helpers.IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public virtual void Impersonate(int userId)
        {
            this._userId = userId;
        }

        public int UserId
        {
            get { return _userId; }
        }

        protected DataContext Db
        {
            get { return _unitOfWork.GetDbContext(); }
        }

        public CacheContext Cache { get; set; }

        protected Bardock.Utils.Logger.ILog Log
        {
            get { return Bardock.Utils.Logger.LogManager.Default.GetLog(this); }
        }

        protected TransactionScope GetTransactionScope(DependentTransaction dtx = null, bool async = false)
        {
            var scopeAsyncOption = async ? TransactionScopeAsyncFlowOption.Enabled : TransactionScopeAsyncFlowOption.Suppress;

            if(dtx == null)
                return new TransactionScope(
                    TransactionScopeOption.Required, 
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                    scopeAsyncOption);
            else
                return new TransactionScope(dtx, scopeAsyncOption);
        }
    }
}
