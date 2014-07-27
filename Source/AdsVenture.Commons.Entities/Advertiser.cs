using AdsVenture.Commons.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdsVenture.Commons.Entities
{
    public class Advertiser : IEntity, ICountryNavigator
    {
        public Guid ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public short CountryID { get; set; }

        public Country Country { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<Content> Contents { get; set; }

        public Advertiser()
        {
            this.Active = true;
            Contents = new HashSet<Content>();
        }

        public short? _CountryID
        {
            get { return this.CountryID; }
        }
    }
}
