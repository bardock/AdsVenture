using AdsVenture.Commons.Entities;
using AdsVenture.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AdsVenture.Core.Managers
{
    public class UserManager : _BaseEntityManager<User>
    {
        public UserManager(
            Helpers.IUnitOfWork unitOfWork)
            : base(unitOfWork) 
        {
        }

        internal IEnumerable<User> FindAll()
        {
            return Db.Users.ToList();
        }
    }
}
