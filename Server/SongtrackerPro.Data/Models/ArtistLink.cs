using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SongtrackerPro.Data.Models
{
    [Table("artist_links")]
    public class ArtistLink
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("artist_id", Order = 2)]
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        [Required]
        [Column("platform_id", Order = 3)]
        public int PlatformId { get; set; }
        public Platform Platform { get; set; }

        [Required]
        [Column("url", Order = 4)]
        public string Url { get; set; }
    }
}
