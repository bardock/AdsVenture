using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Data.Exceptions
{
    public class DuplicatedEntryException : Exception
    {
        public DuplicatedEntryException(Exception innerException)
            : base("Cannot insert duplicate entry", innerException)
        { }
    }
}
