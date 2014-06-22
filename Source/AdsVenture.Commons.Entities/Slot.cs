using AdsVenture.Commons.Entities.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdsVenture.Commons.Entities
{
    public class Slot : IEntity
    {
        public Guid ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public Guid PublisherID { get; set; }

        public Publisher Publisher { get; set; }

        public Guid? ContentID { get; set; }

        public Content Content { get; set; }

        public Slot()
        {
        }
    }
}
