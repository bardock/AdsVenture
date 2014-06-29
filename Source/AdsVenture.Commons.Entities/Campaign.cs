using AdsVenture.Commons.Entities.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdsVenture.Commons.Entities
{
    public class Campaign : IEntity
    {
        public Guid ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public Guid AdvertiserID { get; set; }

        public Advertiser Advertiser { get; set; }

        public DateTime? EndsOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool Active { get; set; }

        public Campaign()
        {
            Active = true;
        }
    }
}
