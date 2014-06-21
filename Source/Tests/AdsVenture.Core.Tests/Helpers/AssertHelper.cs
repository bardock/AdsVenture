using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AdsVenture.Core.Tests.Helpers
{
    public static class AssertHelper
    {
        public static async Task<TException> ThrowsAsync<TException>(Func<Task> func) 
            where TException : Exception
        {
            var expected = typeof(TException);

            Type actual = null;
            Exception ex = null;
            try
            {
                await func();
            }
            catch (Exception e)
            {
                ex = e;
                actual = e.GetType();
            }
            Assert.Equal(typeof(TException), actual);
            return (TException)ex;
        }

        public static async Task<T> DoesNotThrowAsync<TException, T>(Func<Task<T>> f)
            where TException : Exception
        {
            Exception ex = null;
            T result = default(T);
            try
            {
                result = await f();
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.Null(ex);
            return result;
        }
    }
}
