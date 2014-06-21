using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Core.Exceptions
{
    public class EntityNotFoundException : BusinessException
    {
        public EntityNotFoundException()
            : base() { }
        public EntityNotFoundException(string message)
            : base(message) { }
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class EntityNotFoundException<TEntity> : EntityNotFoundException
    {
        public EntityNotFoundException()
            : base() { }
        public EntityNotFoundException(string message)
            : base(message) { }
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}