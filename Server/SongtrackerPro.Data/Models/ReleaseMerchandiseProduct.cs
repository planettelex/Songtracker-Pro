using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    public class ReleaseMerchandiseProduct : MerchandiseProduct
    {
        [JsonIgnore]
        [Column("release_id", Order = 12)]
        public int? ReleaseId { get; set; }
        [JsonIgnore]
        public Release Release { get; set; }

        [Column("media_type", Order = 13)]
        public MediaType? MediaType { get; set; }
    }
}
