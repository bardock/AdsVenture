using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Core.Exceptions
{
    public class EntityAlreadyExistsException<TEntity> : BusinessUserException
    {
        public EntityAlreadyExistsException(string message = null)
            : base(message ?? Resources.BusinessExceptions.EntityAlreadyExistsException) { }
    }
}