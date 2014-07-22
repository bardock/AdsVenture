using AdsVenture.Commons.Entities.DataAnotations;
using AdsVenture.Commons.Entities.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdsVenture.Commons.Entities
{
    public class Advertiser : IEntity
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

        public Advertiser()
        {
            this.Active = true;
        }
    }
}
