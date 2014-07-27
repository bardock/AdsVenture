using System;

namespace AdsVenture.Core.DTO
{
    public class SlotCreate
    {
        public string Title { get; set; }
        public Guid PublisherID { get; set; }
        public Guid? ContentID { get; set; }
    }
}
