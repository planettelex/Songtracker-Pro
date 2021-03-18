using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("artist_links")]
    public class ArtistLink
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("artist_id", Order = 2)]
        public int ArtistId { get; set; }
        [JsonIgnore]
        public Artist Artist { get; set; }

        [JsonIgnore]
        [Required]
        [Column("platform_id", Order = 3)]
        public int PlatformId { get; set; }
        public Platform Platform { get; set; }

        [Required]
        [Column("url", Order = 4)]
        public string Url { get; set; }
    }
}
