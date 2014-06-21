using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Core.Exceptions
{
    /// <summary>
    /// Business exception caused by an invalid user input. 
    /// A valid message is required in order to be displayed to the user.
    /// </summary>
    public class BusinessUserException : BusinessException
    {
        public BusinessUserException(string message)
            : base(message) { }
        public BusinessUserException(string format, params object[] args)
            : base(string.Format(format, args)) { }
        public BusinessUserException(Exception innerException, string message)
            : base(message, innerException) { }
        public BusinessUserException(Exception innerException, string format, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}