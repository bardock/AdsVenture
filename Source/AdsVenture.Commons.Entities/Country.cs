using AdsVenture.Commons.Entities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AdsVenture.Commons.Entities
{
    [DataContract]
    public class Country : IEntity
    {
        public enum Options
        {
            Argentina = 1
        }

        [DataMember]
        public short ID { get; set; }

        [DataMember]
        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [DataMember]
        [Required]
        [MaxLength(2)]
        [Index(IsUnique=true)]
        public string IsoCode { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Country()
        {
            Users = new HashSet<User>();
        }
    }
}
