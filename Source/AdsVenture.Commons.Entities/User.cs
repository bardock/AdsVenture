using AdsVenture.Commons.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdsVenture.Commons.Entities
{
    public class User : IEntity, ICountryNavigator, ILanguageNavigator
    {
        public int ID { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public short? CountryID { get; set; }

        public short LanguageID { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? DateUpdated { get; set; }

        public virtual Country Country { get; set; }

        public virtual Language Language { get; set; }

        //public virtual ICollection<Profile> Profiles { get; set; }

        public User()
        {
            //this.Profiles = new HashSet<Profile>();
        }

        public short? _CountryID
        {
            get { return this.CountryID; }
        }

        public short? _LanguageID
        {
            get { return this.LanguageID; }
        }
    }
}
