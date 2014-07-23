using AdsVenture.Commons.Entities;
using Bardock.Utils.Web.Mvc.HtmlTags;

namespace AdsVenture.Presentation.ContentServer.Models.Publishers
{
    public class Form
    {
        public bool IsNew { get; set; }
        public Publisher Entity { get; set; }
        public OptionsList<Country> CountryOptions { get; set; }
    }
}