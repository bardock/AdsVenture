using HtmlTags;

namespace AdsVenture.Presentation.ContentServer.Models.Shared
{
    public class FileUpload
    {
        public string ID { get; set; }
        public string Url { get; set; }
        public string UrlFieldName { get; set; }
        public string IsEmptyFieldName { get; set; }
        public HtmlTag TextBoxTag { get; set; }
    }
}