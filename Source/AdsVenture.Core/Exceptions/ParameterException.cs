using Bardock.Utils.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Core.Exceptions
{
    public class ParameterException<T> : BusinessException
    {
        private T _param;
        private string _paramName;

        public ParameterException(T param, string paramName = null)
            : base(string.Format(Resources.BusinessExceptions.ParameterNullException, paramName ?? typeof(T).Name))
        {
            this._param = param;
            this._paramName = paramName;
        }
    }

    public static class ParameterException
    {
        public static ParameterException<T> Create<T>(T param, string paramName = null)
        {
            return new ParameterException<T>(param, paramName);
        }
        //TODO: Use expression<func<TParam>> to catch paramName and the ExpressionHelper to get the string
        //public static ParameterException<T> Create<T>(Expression<Func<T>> expr)
        //{
        //    return new ParameterException<T>(expr.Compile().Invoke(), ExpressionHelper.GetExpressionText(expr));
        //}
    }
}
