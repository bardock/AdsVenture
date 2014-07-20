using System;

namespace AdsVenture.Core.DTO
{
    public class AdvertiserCreate
    {
        public string Name { get; set; }

        public short CountryID { get; set; }

        public bool Active { get; set; }
    }
}
