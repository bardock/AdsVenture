using AdsVenture.Commons.Entities;
using Bardock.Utils.Web.Mvc.HtmlTags;

namespace AdsVenture.Presentation.ContentServer.Models.Slots
{
    public class Form
    {
        public bool IsNew { get; set; }
        public Slot Entity { get; set; }
        public OptionsList<Publisher> PublisherOptions { get; set; }
        public OptionsList<Content> ContentOptions { get; set; }
    }
}