using AdsVenture.Commons.Entities.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdsVenture.Commons.Entities
{
    public enum SlotEventDiscriminator
    {
        Impression = 1,
        UserInteraction = 2
    }

    public class SlotEvent : IEntity
    {
        public Guid ID { get; set; }

        public SlotEventDiscriminator Discriminator { get; set; }

        public Guid SlotID { get; set; }

        public Slot Slot { get; set; }

        public Guid ContentID { get; set; }

        public Content Content { get; set; }

        public Guid? CampaignID { get; set; }

        public Campaign Campaign { get; set; }

        [MaxLength(10)]
        public string EventType { get; set; }

        public int? PositionX { get; set; }

        public int? PositionY { get; set; }

        public virtual SlotEventTarget Target { get; set; }

        public DateTime Date { get; set; }

        public SlotEvent()
        {
            Target = new SlotEventTarget();
        }
    }

    public class SlotEventTarget
    {
        public Guid ID { get; set; }

        [MaxLength(10)]
        public string TagName { get; set; }

        [MaxLength(50)]
        public string ElemId { get; set; }

        [MaxLength(50)]
        public string ElemClass { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string Type { get; set; }

        [MaxLength(50)]
        public string Value { get; set; }

        [MaxLength(255)]
        public string Href { get; set; }

        [MaxLength(255)]
        public string Onclick { get; set; }

        [MaxLength(255)]
        public string Action { get; set; }

        [MaxLength(10)]
        public string Method { get; set; }
    }
}
