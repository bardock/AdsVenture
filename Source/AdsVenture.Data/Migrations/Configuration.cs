namespace AdsVenture.Data.Migrations
{
    using AdsVenture.Commons.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using AdsVenture.Data.Helpers;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<AdsVenture.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AdsVenture.Data.DataContext context)
        {
            if (!context.Countries.Any())
            {
                context.Countries.AddOrUpdate(
                    new Country() { ID = 1, IsoCode = "AR", Description = "Argentina" },
                    new Country() { ID = 2, IsoCode = "US", Description = "Estados Unidos" },
                    new Country() { ID = 3, IsoCode = "BR", Description = "Brasil" },
                    new Country() { ID = 4, IsoCode = "CL", Description = "Chile" },
                    new Country() { ID = 5, IsoCode = "CO", Description = "Colombia" },
                    new Country() { ID = 6, IsoCode = "MX", Description = "México" },
                    new Country() { ID = 7, IsoCode = "GT", Description = "Guatemala" },
                    new Country() { ID = 8, IsoCode = "VE", Description = "Venezuela" },
                    new Country() { ID = 9, IsoCode = "AF", Description = "Afghanistan" },
                    new Country() { ID = 10, IsoCode = "AL", Description = "Albania" },
                    new Country() { ID = 11, IsoCode = "DZ", Description = "Algeria" },
                    new Country() { ID = 12, IsoCode = "AS", Description = "American Samoa" },
                    new Country() { ID = 13, IsoCode = "AD", Description = "Andorra" },
                    new Country() { ID = 14, IsoCode = "AO", Description = "Angola" },
                    new Country() { ID = 15, IsoCode = "AI", Description = "Anguilla" },
                    new Country() { ID = 16, IsoCode = "AQ", Description = "Antarctica" },
                    new Country() { ID = 17, IsoCode = "AG", Description = "Antigua and Barbuda" },
                    new Country() { ID = 18, IsoCode = "AM", Description = "Armenia" },
                    new Country() { ID = 19, IsoCode = "AW", Description = "Aruba" },
                    new Country() { ID = 20, IsoCode = "AP", Description = "Asia/Pacific Region" },
                    new Country() { ID = 21, IsoCode = "AU", Description = "Australia" },
                    new Country() { ID = 22, IsoCode = "AT", Description = "Austria" },
                    new Country() { ID = 23, IsoCode = "AZ", Description = "Azerbaijan" },
                    new Country() { ID = 24, IsoCode = "BS", Description = "Bahamas" },
                    new Country() { ID = 25, IsoCode = "BH", Description = "Bahrain" },
                    new Country() { ID = 26, IsoCode = "BD", Description = "Bangladesh" },
                    new Country() { ID = 27, IsoCode = "BB", Description = "Barbados" },
                    new Country() { ID = 28, IsoCode = "BY", Description = "Belarus" },
                    new Country() { ID = 29, IsoCode = "BE", Description = "Belgium" },
                    new Country() { ID = 30, IsoCode = "BZ", Description = "Belize" },
                    new Country() { ID = 31, IsoCode = "BJ", Description = "Benin" },
                    new Country() { ID = 32, IsoCode = "BM", Description = "Bermuda" },
                    new Country() { ID = 33, IsoCode = "BT", Description = "Bhutan" },
                    new Country() { ID = 34, IsoCode = "BO", Description = "Bolivia" },
                    new Country() { ID = 35, IsoCode = "BA", Description = "Bosnia and Herzegovina" },
                    new Country() { ID = 36, IsoCode = "BW", Description = "Botswana" },
                    new Country() { ID = 37, IsoCode = "BV", Description = "Bouvet Island" },
                    new Country() { ID = 38, IsoCode = "IO", Description = "British Indian Ocean Territory" },
                    new Country() { ID = 39, IsoCode = "BN", Description = "Brunei Darussalam" },
                    new Country() { ID = 40, IsoCode = "BG", Description = "Bulgaria" },
                    new Country() { ID = 41, IsoCode = "BF", Description = "Burkina Faso" },
                    new Country() { ID = 42, IsoCode = "BI", Description = "Burundi" },
                    new Country() { ID = 43, IsoCode = "KH", Description = "Cambodia" },
                    new Country() { ID = 44, IsoCode = "CM", Description = "Cameroon" },
                    new Country() { ID = 45, IsoCode = "CA", Description = "Canada" },
                    new Country() { ID = 46, IsoCode = "CV", Description = "Cape Verde" },
                    new Country() { ID = 47, IsoCode = "KY", Description = "Cayman Islands" },
                    new Country() { ID = 48, IsoCode = "CF", Description = "Central African Republic" },
                    new Country() { ID = 49, IsoCode = "TD", Description = "Chad" },
                    new Country() { ID = 50, IsoCode = "CN", Description = "China" },
                    new Country() { ID = 51, IsoCode = "CX", Description = "Christmas Island" },
                    new Country() { ID = 52, IsoCode = "CC", Description = "Cocos (Keeling) Islands" },
                    new Country() { ID = 53, IsoCode = "KM", Description = "Comoros" },
                    new Country() { ID = 54, IsoCode = "CG", Description = "Congo" },
                    new Country() { ID = 55, IsoCode = "CD", Description = "Congo, the Democratic Republic of the" },
                    new Country() { ID = 56, IsoCode = "CK", Description = "Cook Islands" },
                    new Country() { ID = 57, IsoCode = "CR", Description = "Costa Rica" },
                    new Country() { ID = 58, IsoCode = "HR", Description = "Croatia" },
                    new Country() { ID = 59, IsoCode = "CY", Description = "Cyprus" },
                    new Country() { ID = 60, IsoCode = "CZ", Description = "Czech Republic" },
                    new Country() { ID = 61, IsoCode = "DK", Description = "Denmark" },
                    new Country() { ID = 62, IsoCode = "DJ", Description = "Djibouti" },
                    new Country() { ID = 63, IsoCode = "DM", Description = "Dominica" },
                    new Country() { ID = 64, IsoCode = "DO", Description = "Dominican Republic" },
                    new Country() { ID = 65, IsoCode = "TL", Description = "East Timor" },
                    new Country() { ID = 66, IsoCode = "EC", Description = "Ecuador" },
                    new Country() { ID = 67, IsoCode = "EG", Description = "Egypt" },
                    new Country() { ID = 68, IsoCode = "SV", Description = "El Salvador" },
                    new Country() { ID = 69, IsoCode = "GQ", Description = "Equatorial Guinea" },
                    new Country() { ID = 70, IsoCode = "ER", Description = "Eritrea" },
                    new Country() { ID = 71, IsoCode = "EE", Description = "Estonia" },
                    new Country() { ID = 72, IsoCode = "ET", Description = "Ethiopia" },
                    new Country() { ID = 73, IsoCode = "EU", Description = "Europe" },
                    new Country() { ID = 74, IsoCode = "FK", Description = "Falkland Islands (Malvinas)" },
                    new Country() { ID = 75, IsoCode = "FO", Description = "Faroe Islands" },
                    new Country() { ID = 76, IsoCode = "FJ", Description = "Fiji" },
                    new Country() { ID = 77, IsoCode = "FI", Description = "Finland" },
                    new Country() { ID = 78, IsoCode = "FR", Description = "France" },
                    new Country() { ID = 79, IsoCode = "GF", Description = "French Guiana" },
                    new Country() { ID = 80, IsoCode = "PF", Description = "French Polynesia" },
                    new Country() { ID = 81, IsoCode = "TF", Description = "French Southern Territories" },
                    new Country() { ID = 82, IsoCode = "GA", Description = "Gabon" },
                    new Country() { ID = 83, IsoCode = "GM", Description = "Gambia" },
                    new Country() { ID = 84, IsoCode = "GE", Description = "Georgia" },
                    new Country() { ID = 85, IsoCode = "DE", Description = "Germany" },
                    new Country() { ID = 86, IsoCode = "GH", Description = "Ghana" },
                    new Country() { ID = 87, IsoCode = "GI", Description = "Gibraltar" },
                    new Country() { ID = 88, IsoCode = "GR", Description = "Greece" },
                    new Country() { ID = 89, IsoCode = "GL", Description = "Greenland" },
                    new Country() { ID = 90, IsoCode = "GD", Description = "Grenada" },
                    new Country() { ID = 91, IsoCode = "GP", Description = "Guadeloupe" },
                    new Country() { ID = 92, IsoCode = "GU", Description = "Guam" },
                    new Country() { ID = 93, IsoCode = "GN", Description = "Guinea" },
                    new Country() { ID = 94, IsoCode = "GW", Description = "Guinea-Bissau" },
                    new Country() { ID = 95, IsoCode = "GY", Description = "Guyana" },
                    new Country() { ID = 96, IsoCode = "HT", Description = "Haiti" },
                    new Country() { ID = 97, IsoCode = "HM", Description = "Heard Island and McDonald Islands" },
                    new Country() { ID = 98, IsoCode = "VA", Description = "Holy See (Vatican City State)" },
                    new Country() { ID = 99, IsoCode = "HN", Description = "Honduras" },
                    new Country() { ID = 100, IsoCode = "HK", Description = "Hong Kong" },
                    new Country() { ID = 101, IsoCode = "HU", Description = "Hungary" },
                    new Country() { ID = 102, IsoCode = "IS", Description = "Iceland" },
                    new Country() { ID = 103, IsoCode = "IN", Description = "India" },
                    new Country() { ID = 104, IsoCode = "ID", Description = "Indonesia" },
                    new Country() { ID = 105, IsoCode = "IQ", Description = "Iraq" },
                    new Country() { ID = 106, IsoCode = "IE", Description = "Ireland" },
                    new Country() { ID = 107, IsoCode = "IL", Description = "Israel" },
                    new Country() { ID = 108, IsoCode = "IT", Description = "Italy" },
                    new Country() { ID = 109, IsoCode = "CI", Description = "Ivory Coast" },
                    new Country() { ID = 110, IsoCode = "JM", Description = "Jamaica" },
                    new Country() { ID = 111, IsoCode = "JP", Description = "Japan" },
                    new Country() { ID = 112, IsoCode = "JO", Description = "Jordan" },
                    new Country() { ID = 113, IsoCode = "KZ", Description = "Kazakhstan" },
                    new Country() { ID = 114, IsoCode = "KE", Description = "Kenya" },
                    new Country() { ID = 115, IsoCode = "KI", Description = "Kiribati" },
                    new Country() { ID = 116, IsoCode = "KR", Description = "Korea- Republic of" },
                    new Country() { ID = 117, IsoCode = "KW", Description = "Kuwait" },
                    new Country() { ID = 118, IsoCode = "KG", Description = "Kyrgyzstan" },
                    new Country() { ID = 119, IsoCode = "LA", Description = "Lao Peoples Democratic Republic" },
                    new Country() { ID = 120, IsoCode = "LV", Description = "Latvia" },
                    new Country() { ID = 121, IsoCode = "LB", Description = "Lebanon" },
                    new Country() { ID = 122, IsoCode = "LS", Description = "Lesotho" },
                    new Country() { ID = 123, IsoCode = "LR", Description = "Liberia" },
                    new Country() { ID = 124, IsoCode = "LY", Description = "Libyan Arab Jamahiriya" },
                    new Country() { ID = 125, IsoCode = "LI", Description = "Liechtenstein" },
                    new Country() { ID = 126, IsoCode = "LT", Description = "Lithuania" },
                    new Country() { ID = 127, IsoCode = "LU", Description = "Luxembourg" },
                    new Country() { ID = 128, IsoCode = "MO", Description = "Macao" },
                    new Country() { ID = 129, IsoCode = "MK", Description = "Macedonia" },
                    new Country() { ID = 130, IsoCode = "MG", Description = "Madagascar" },
                    new Country() { ID = 131, IsoCode = "MW", Description = "Malawi" },
                    new Country() { ID = 132, IsoCode = "MY", Description = "Malaysia" },
                    new Country() { ID = 133, IsoCode = "MV", Description = "Maldives" },
                    new Country() { ID = 134, IsoCode = "ML", Description = "Mali" },
                    new Country() { ID = 135, IsoCode = "MT", Description = "Malta" },
                    new Country() { ID = 136, IsoCode = "MH", Description = "Marshall Islands" },
                    new Country() { ID = 137, IsoCode = "MQ", Description = "Martinique" },
                    new Country() { ID = 138, IsoCode = "MR", Description = "Mauritania" },
                    new Country() { ID = 139, IsoCode = "MU", Description = "Mauritius" },
                    new Country() { ID = 140, IsoCode = "YT", Description = "Mayotte" },
                    new Country() { ID = 141, IsoCode = "FM", Description = "Micronesia- Federated States of" },
                    new Country() { ID = 142, IsoCode = "MD", Description = "Moldova- Republic of" },
                    new Country() { ID = 143, IsoCode = "MC", Description = "Monaco" },
                    new Country() { ID = 144, IsoCode = "MN", Description = "Mongolia" },
                    new Country() { ID = 145, IsoCode = "ME", Description = "Montenegro" },
                    new Country() { ID = 146, IsoCode = "MS", Description = "Montserrat" },
                    new Country() { ID = 147, IsoCode = "MA", Description = "Morocco" },
                    new Country() { ID = 148, IsoCode = "MZ", Description = "Mozambique" },
                    new Country() { ID = 149, IsoCode = "MM", Description = "Myanmar" },
                    new Country() { ID = 150, IsoCode = "NA", Description = "Namibia" },
                    new Country() { ID = 151, IsoCode = "NR", Description = "Nauru" },
                    new Country() { ID = 152, IsoCode = "NP", Description = "Nepal" },
                    new Country() { ID = 153, IsoCode = "NL", Description = "Netherlands" },
                    new Country() { ID = 154, IsoCode = "AN", Description = "Netherlands Antilles" },
                    new Country() { ID = 155, IsoCode = "NC", Description = "New Caledonia" },
                    new Country() { ID = 156, IsoCode = "NZ", Description = "New Zealand" },
                    new Country() { ID = 157, IsoCode = "NI", Description = "Nicaragua" },
                    new Country() { ID = 158, IsoCode = "NE", Description = "Niger" },
                    new Country() { ID = 159, IsoCode = "NG", Description = "Nigeria" },
                    new Country() { ID = 160, IsoCode = "NU", Description = "Niue" },
                    new Country() { ID = 161, IsoCode = "NF", Description = "Norfolk Island" },
                    new Country() { ID = 162, IsoCode = "MP", Description = "Northern Mariana Islands" },
                    new Country() { ID = 163, IsoCode = "NO", Description = "Norway" },
                    new Country() { ID = 164, IsoCode = "OM", Description = "Oman" },
                    new Country() { ID = 165, IsoCode = "PK", Description = "Pakistan" },
                    new Country() { ID = 166, IsoCode = "PW", Description = "Palau" },
                    new Country() { ID = 167, IsoCode = "PS", Description = "Palestine" },
                    new Country() { ID = 168, IsoCode = "PA", Description = "Panama" },
                    new Country() { ID = 169, IsoCode = "PG", Description = "Papua New Guinea" },
                    new Country() { ID = 170, IsoCode = "PY", Description = "Paraguay" },
                    new Country() { ID = 171, IsoCode = "PE", Description = "Peru" },
                    new Country() { ID = 172, IsoCode = "PH", Description = "Philippines" },
                    new Country() { ID = 173, IsoCode = "PN", Description = "Pitcairn" },
                    new Country() { ID = 174, IsoCode = "PL", Description = "Poland" },
                    new Country() { ID = 175, IsoCode = "PT", Description = "Portugal" },
                    new Country() { ID = 176, IsoCode = "PR", Description = "Puerto Rico" },
                    new Country() { ID = 177, IsoCode = "QA", Description = "Qatar" },
                    new Country() { ID = 178, IsoCode = "RE", Description = "Reunion" },
                    new Country() { ID = 179, IsoCode = "RO", Description = "Romania" },
                    new Country() { ID = 180, IsoCode = "RU", Description = "Russian Federation" },
                    new Country() { ID = 181, IsoCode = "RW", Description = "Rwanda" },
                    new Country() { ID = 182, IsoCode = "SH", Description = "Saint Helena" },
                    new Country() { ID = 183, IsoCode = "KN", Description = "Saint Kitts and Nevis" },
                    new Country() { ID = 184, IsoCode = "LC", Description = "Saint Lucia" },
                    new Country() { ID = 185, IsoCode = "PM", Description = "Saint Pierre and Miquelon" },
                    new Country() { ID = 186, IsoCode = "VC", Description = "Saint Vincent and the Grenadines" },
                    new Country() { ID = 187, IsoCode = "WS", Description = "Samoa" },
                    new Country() { ID = 188, IsoCode = "SM", Description = "San Marino" },
                    new Country() { ID = 189, IsoCode = "ST", Description = "Sao Tome and Principe" },
                    new Country() { ID = 190, IsoCode = "SA", Description = "Saudi Arabia" },
                    new Country() { ID = 191, IsoCode = "SN", Description = "Senegal" },
                    new Country() { ID = 192, IsoCode = "RS", Description = "Serbia" },
                    new Country() { ID = 193, IsoCode = "SC", Description = "Seychelles" },
                    new Country() { ID = 194, IsoCode = "SL", Description = "Sierra Leone" },
                    new Country() { ID = 195, IsoCode = "SG", Description = "Singapore" },
                    new Country() { ID = 196, IsoCode = "SK", Description = "Slovakia" },
                    new Country() { ID = 197, IsoCode = "SI", Description = "Slovenia" },
                    new Country() { ID = 198, IsoCode = "SB", Description = "Solomon Islands" },
                    new Country() { ID = 199, IsoCode = "SO", Description = "Somalia" },
                    new Country() { ID = 200, IsoCode = "ZA", Description = "South Africa" },
                    new Country() { ID = 201, IsoCode = "GS", Description = "South Georgia and the South Sandwich Islands" },
                    new Country() { ID = 202, IsoCode = "ES", Description = "Spain" },
                    new Country() { ID = 203, IsoCode = "LK", Description = "Sri Lanka" },
                    new Country() { ID = 204, IsoCode = "SR", Description = "Suriname" },
                    new Country() { ID = 205, IsoCode = "SJ", Description = "Svalbard and Jan Mayen" },
                    new Country() { ID = 206, IsoCode = "SZ", Description = "Swaziland" },
                    new Country() { ID = 207, IsoCode = "SE", Description = "Sweden" },
                    new Country() { ID = 208, IsoCode = "CH", Description = "Switzerland" },
                    new Country() { ID = 209, IsoCode = "TW", Description = "Taiwan- Republic of China" },
                    new Country() { ID = 210, IsoCode = "TJ", Description = "Tajikistan" },
                    new Country() { ID = 211, IsoCode = "TZ", Description = "Tanzania- United Republic of" },
                    new Country() { ID = 212, IsoCode = "TH", Description = "Thailand" },
                    new Country() { ID = 213, IsoCode = "TG", Description = "Togo" },
                    new Country() { ID = 214, IsoCode = "TK", Description = "Tokelau" },
                    new Country() { ID = 215, IsoCode = "TO", Description = "Tonga" },
                    new Country() { ID = 216, IsoCode = "TT", Description = "Trinidad and Tobago" },
                    new Country() { ID = 217, IsoCode = "TN", Description = "Tunisia" },
                    new Country() { ID = 218, IsoCode = "TR", Description = "Turkey" },
                    new Country() { ID = 219, IsoCode = "TM", Description = "Turkmenistan" },
                    new Country() { ID = 220, IsoCode = "TC", Description = "Turks and Caicos Islands" },
                    new Country() { ID = 221, IsoCode = "TV", Description = "Tuvalu" },
                    new Country() { ID = 222, IsoCode = "UG", Description = "Uganda" },
                    new Country() { ID = 223, IsoCode = "UA", Description = "Ukraine" },
                    new Country() { ID = 224, IsoCode = "AE", Description = "United Arab Emirates" },
                    new Country() { ID = 225, IsoCode = "GB", Description = "United Kingdom" },
                    new Country() { ID = 226, IsoCode = "UM", Description = "United States Minor Outlying Islands" },
                    new Country() { ID = 227, IsoCode = "UY", Description = "Uruguay" },
                    new Country() { ID = 228, IsoCode = "UZ", Description = "Uzbekistan" },
                    new Country() { ID = 229, IsoCode = "VU", Description = "Vanuatu" },
                    new Country() { ID = 230, IsoCode = "VN", Description = "Vietnam" },
                    new Country() { ID = 231, IsoCode = "VG", Description = "Virgin Islands- British" },
                    new Country() { ID = 232, IsoCode = "VI", Description = "Virgin Islands- U.S." },
                    new Country() { ID = 233, IsoCode = "WF", Description = "Wallis and Futuna" },
                    new Country() { ID = 234, IsoCode = "EH", Description = "Western Sahara" },
                    new Country() { ID = 235, IsoCode = "YE", Description = "Yemen" },
                    new Country() { ID = 236, IsoCode = "YU", Description = "Yugoslavia" },
                    new Country() { ID = 237, IsoCode = "ZR", Description = "Zaire" },
                    new Country() { ID = 238, IsoCode = "ZM", Description = "Zambia" },
                    new Country() { ID = 239, IsoCode = "ZW", Description = "Zimbabwe" }
                );
                context.SaveChanges();
            }
            if (!context.Languages.Any())
            {
                context.Languages.AddOrUpdate(
                    new Language()
                    {
                        ID = (short)Language.Options.EN,
                        IsoCode = Language.Options.EN.ToString(),
                        Description = "English"
                    },
                    new Language()
                    {
                        ID = (short)Language.Options.ES,
                        IsoCode = Language.Options.ES.ToString(),
                        Description = "Español"
                    },
                    new Language()
                    {
                        ID = (short)Language.Options.PT,
                        IsoCode = Language.Options.PT.ToString(),
                        Description = "Português"
                    }
                );
                context.SaveChanges();
            }

            SeedTestData(context);
        }

        private void SeedTestData(DataContext context)
        {
            var adv1 = new Advertiser()
            {
                ID = Guid.Parse("dd8e88b7473844eba101aab216e5bed1"),
                Name = "adv1",
                CountryID = 1,
                CreatedOn = new DateTime(2014,1,1)
            };
            context.Advertisers.AddOrUpdate(adv1);
            context.SaveChanges();

            var pub1 = new Publisher()
            {
                ID = Guid.Parse("66b0cd5e1c214307a5192204022dbceb"),
                Name = "pub1",
                CountryID = 1,
                CreatedOn = new DateTime(2014,1,1)
            };
            context.Publishers.AddOrUpdate(pub1);
            context.SaveChanges();

            var contentRef1 = new Content()
            {
                ID = Guid.Parse("27c26f25af8d4e0d9004517aa31aaf16"),
                Title = "Ref1",
                AdvertiserID = adv1.ID,
                Url = "//adv1.content.avt.com/ContentReference/SampleFluidExternal",
                CreatedOn = new DateTime(2014, 1, 1)
            };
            context.Contents.AddOrUpdate(contentRef1);
            context.SaveChanges();

            context.Campaigns.AddOrUpdate(
                new Campaign()
                {
                    ID = Guid.Parse("1b8dafc4369e74e5535711d654bb2f41"),
                    Title = "Campaign1",
                    AdvertiserID = adv1.ID,
                    CreatedOn = DateTime.UtcNow
                }
            );
            context.SaveChanges();

            context.Slots.AddOrUpdate(
                new Slot()
                {
                    ID = Guid.Parse("5711d654bb2f411b8dafc4369e74e553"),
                    Title = "Slot1",
                    PublisherID = pub1.ID,
                    ContentID = contentRef1.ID,
                    CreatedOn = new DateTime(2014, 1, 1)
                }
            );
            context.SaveChanges();
        }
    }
}
