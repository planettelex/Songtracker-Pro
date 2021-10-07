using System.ComponentModel.DataAnnotations.Schema;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    public class Business : LegalEntity
    {
        public Business()
        {
            EntityType = LegalEntityType.Company;
        }

        [Column("trade_name", Order = 20)]
        public string TradeName { get; set; }

        [Column("website_url", Order = 21)]
        public string WebsiteUrl { get; set; }

        [Column("has_servicemark", Order = 22)]
        public bool? HasServicemark { get; set; }

        [Column("has_trademark", Order = 23)]
        public bool? HasTrademark { get; set; }
    }
}
