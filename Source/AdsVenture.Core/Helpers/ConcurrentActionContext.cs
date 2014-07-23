using System;
using System.Threading.Tasks;
using Bardock.Utils.Logger;

namespace AdsVenture.Core.Helpers
{
    public static class ConcurrentActionContext
    {
        /// <summary>
        /// Creates and executes a ConcurrentActionContext
        /// </summary>
        public static async Task<TReturn> ExecAsync<TReturn>(
            Func<ConcurrentActionContext<TReturn>, Task<TReturn>> mainAction,
            int? maxAttemps = null)
        {
            return await (new ConcurrentActionContext<TReturn>(mainAction, maxAttemps)).ExecAsync();
        }

        /// <summary>
        /// Creates and executes a ConcurrentActionContext
        /// </summary>
        public static async Task<TReturn> ExecAsync<TReturn>(
            Func<ConcurrentActionContext<TReturn>, TReturn> mainAction,
            int? maxAttemps = null)
        {
            return await (new ConcurrentActionContext<TReturn>(mainAction, maxAttemps)).ExecAsync();
        }
    }

    /// <summary>
    /// Manages an action that can produce concurrency exception (i.e. DbUpdateConcurrencyException).
    /// Uses optimistic concurrency control and retries the action multiple times.
    /// </summary>
    public class ConcurrentActionContext<TReturn>
    {
        private Func<ConcurrentActionContext<TReturn>, Task<TReturn>> _mainAction;
        private int _maxAttemps;
        private int _attemps;

        /// <summary>
        /// Event raised after main action succeded
        /// </summary>
        public event Action OnSuccess;

        /// <summary>
        /// Event raised when main action produces a concurrency error
        /// </summary>
        public event Action OnError;

        /// <summary>
        /// Event raised always after main action
        /// </summary>
        public event Action OnComplete;

        /// <param name="mainAction">The main action that needs concurrency control. Receives this instance as parameter.</param>
        /// <param name="maxAttemps">The maximum of attempts to retry the main action in case of concurrency exceptions</param>
        public ConcurrentActionContext(
            Func<ConcurrentActionContext<TReturn>, Task<TReturn>> mainAction,
            int? maxAttemps = null)
        {
            this._mainAction = mainAction;
            this._maxAttemps = maxAttemps ?? ConfigSection.Default.ConcurrentActionContext.DefaultMaxAttemps;
            this._attemps = 0;
        }

        /// <param name="mainAction">The main action that needs concurrency control. Receives this instance as parameter.</param>
        /// <param name="finallyAction">Action to be executed always after main action</param>
        /// <param name="maxAttemps">The maximum of attempts to retry the main action in case of concurrency exceptions</param>
        public ConcurrentActionContext(
            Func<ConcurrentActionContext<TReturn>, TReturn> mainAction,
            int? maxAttemps = null)
        : this(
            ctx => Task.Run(() => mainAction(ctx)), 
            maxAttemps)
        { }

        public async Task<TReturn> ExecAsync()
        {
            Exception ex;
            try
            {
                var result = await _mainAction(this);
                RaiseOnSuccess();
                RaiseOnComplete();
                return result;
            }
            catch (Data.Exceptions.DuplicatedEntryException deEx)
            {
                ex = deEx;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException dbucEx)
            {
                ex = dbucEx;
            }
            return await HandleException(ex);
        }

        private async Task<TReturn> HandleException(Exception ex)
        {
            Log.Debug(string.Format("Concurrency Error. Attemp #{0}", _attemps), ex);

            RaiseOnError();
            RaiseOnComplete();

            _attemps++;
            if (_attemps >= _maxAttemps)
                throw ex;

            return await ExecAsync();
        }

        private void RaiseOnSuccess()
        {
            var onSuccess = this.OnSuccess;
            if (onSuccess != null)
                onSuccess();
        }

        private void RaiseOnError()
        {
            var onError = this.OnError;
            if (onError != null)
                onError();
        }

        private void RaiseOnComplete()
        {
            var onComplete = this.OnComplete;
            if (onComplete != null)
                onComplete();
        }

        public ILog Log { get { return LogManager.Default.GetLog(this); } }
    }
}
