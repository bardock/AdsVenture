using AdsVenture.Commons.Entities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AdsVenture.Commons.Entities
{
    [DataContract]
    public class Language : IEntity
    {
        public enum Options
        {
            EN = 1,
            ES = 2,
            PT = 3
        }

        [DataMember]
        public short ID { get; set; }

        [DataMember]
        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [DataMember]
        [MaxLength(3)]
        [Index(IsUnique=true)]
        public string IsoCode { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Language()
        {
            Users = new HashSet<User>();
        }
    }
}
