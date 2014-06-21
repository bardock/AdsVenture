using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Core.Exceptions
{
    public class UnauthorizedException : BusinessException
    {
        public UnauthorizedException()
            : base() { }
        public UnauthorizedException(string message)
            : base(message) { }
        public UnauthorizedException(string format, params object[] args)
            : this(string.Format(format, args)) { }
        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}