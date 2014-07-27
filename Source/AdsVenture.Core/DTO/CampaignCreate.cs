using System;

namespace AdsVenture.Core.DTO
{
    public class CampaignCreate
    {
        public string Title { get; set; }
        public Guid AdvertiserID { get; set; }
        public DateTime? EndsOn { get; set; }
    }
}
