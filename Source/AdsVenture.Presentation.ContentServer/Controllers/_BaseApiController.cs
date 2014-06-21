using System.Transactions;
using Bardock.Utils.Web.Mvc;

namespace AdsVenture.Presentation.ContentServer.Controllers
{
    public class _BaseApiController : BaseApiController
    {
        protected TransactionScope GetTransactionScope()
        {
            return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}
