using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdsVenture.Commons.Entities;
using AdsVenture.Core.Validation;

namespace AdsVenture.Core.Managers
{
    public class CountryManager : _BaseEntityManager<Country>
    {
        public CountryManager(Helpers.IUnitOfWork unitOfWork)
            : base(unitOfWork) { }

        public virtual List<Country> FindAll()
        {
            return Cache.Countries.GetData();
        }

        public virtual Country Find(int id)
        {
            return FindAll().FirstOrDefault(x => x.ID == id);
        }

        public virtual Country FindByIsoCode(string isoCode)
        {
            isoCode = isoCode.ToUpper().Trim();
            return FindAll().FirstOrDefault(x => x.IsoCode.ToUpper() == isoCode);
        }
    }
}