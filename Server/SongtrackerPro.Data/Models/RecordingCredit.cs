using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("recording_credits")]
    public class RecordingCredit
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("recording_id", Order = 2)]
        public int RecordingId { get; set; }
        [JsonIgnore]
        public Recording Recording { get; set; }

        [JsonIgnore]
        [Required]
        [Column("person_id", Order = 3)]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        [Required]
        [Column("is_featured", Order = 4)]
        public bool IsFeatured { get; set; }

        [Column("ownership_percentage", Order = 5)]
        public decimal? OwnershipPercentage { get; set; }
    }
}
