using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    public class ReleaseMerchandiseProduct : MerchandiseProduct
    {
        [JsonIgnore]
        [Column("release_id", Order = 11)]
        public int? ReleaseId { get; set; }
        [JsonIgnore]
        public Release Release { get; set; }

        [Column("media_type", Order = 12)]
        public MediaType? Type { get; set; }
    }
}
