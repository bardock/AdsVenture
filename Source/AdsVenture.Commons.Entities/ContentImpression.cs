using AdsVenture.Commons.Entities.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdsVenture.Commons.Entities
{
    public class ContentImpression : IEntity
    {
        public long ID { get; set; }

        public Guid ContentID { get; set; }

        public DateTime CreatedOn { get; set; }

        public ContentImpression()
        {
        }
    }
}
