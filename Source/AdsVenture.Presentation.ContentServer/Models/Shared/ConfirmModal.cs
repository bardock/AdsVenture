using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdsVenture.Presentation.ContentServer.Models.Shared
{
    public class ConfirmModal
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string OnConfirm { get; set; }
    }
}