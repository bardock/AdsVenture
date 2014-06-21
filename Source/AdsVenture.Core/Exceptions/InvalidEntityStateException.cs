using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Core.Exceptions
{
    public class InvalidEntityStateException : BusinessException
    {
        public InvalidEntityStateException()
            : base()
        { }
        public InvalidEntityStateException(string message)
            : base(message)
        { }
        public InvalidEntityStateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
