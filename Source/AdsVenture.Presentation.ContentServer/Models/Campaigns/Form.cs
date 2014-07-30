using AdsVenture.Commons.Entities;
using Bardock.Utils.Web.Mvc.HtmlTags;
using System.Collections.Generic;

namespace AdsVenture.Presentation.ContentServer.Models.Campaigns
{
    public class Form
    {
        public bool IsNew { get; set; }
        public Campaign Entity { get; set; }
        public OptionsList<Advertiser> AdvertiserOptions { get; set; }
        public IEnumerable<SlotEvent> Events { get; set; }
    }
}