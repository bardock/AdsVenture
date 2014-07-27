using AdsVenture.Commons.Entities;
using Bardock.Utils.Web.Mvc.HtmlTags;

namespace AdsVenture.Presentation.ContentServer.Models.Contents
{
    public class Form
    {
        public bool IsNew { get; set; }
        public Content Entity { get; set; }
        public OptionsList<Advertiser> AdvertiserOptions { get; set; }
    }
}