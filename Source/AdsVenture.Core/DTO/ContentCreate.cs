using System;

namespace AdsVenture.Core.DTO
{
    public class ContentCreate
    {
        public string Title { get; set; }
        public Guid AdvertiserID { get; set; }
        public string Url { get; set; }
    }
}
