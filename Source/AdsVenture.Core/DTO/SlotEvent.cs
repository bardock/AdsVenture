using System;

namespace AdsVenture.Core.DTO
{
    public class SlotUserEvent
    {
        public Guid SlotID { get; set; }

        public Guid ContentID { get; set; }

        public string EventType { get; set; }

        public int? PositionX { get; set; }

        public int? PositionY { get; set; }

        public virtual SlotEventTarget Target { get; set; }

        public DateTime Date { get; set; }
    }

    public class SlotEventTarget
    {
        public string TagName { get; set; }

        public string ElemId { get; set; }

        public string ElemClass { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public string Href { get; set; }

        public string Onclick { get; set; }

        public string Action { get; set; }

        public string Method { get; set; }
    }
}
