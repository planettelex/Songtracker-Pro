using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    [Table("release_media")]
    public class ReleaseMedia
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("release_id", Order = 2)]
        public int ReleaseId { get; set; }
        [JsonIgnore]
        public Release Release { get; set; }

        [Required]
        [Column("type", Order = 3)]
        public MediaType Type { get; set; }

        [Column("sku", Order = 4)]
        public string Sku { get; set; }

        [Column("upc", Order = 5)]
        public string Upc { get; set; }
    }
}
