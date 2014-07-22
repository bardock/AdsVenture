using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdsVenture.Presentation.ContentServer.Models.Shared
{
    // <summary>
    // This class is used for posting an array of IDs to a WebApi action
    // </summary>
    public class MassiveParams<TId>
    {
        public TId[] IDs { get; set; }
    }

    // <summary>
    // This class is used for posting an array of IDs to a WebApi action
    // </summary>
    public class MassiveParams : MassiveParams<int>
    {
    }
}