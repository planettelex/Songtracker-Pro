using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("release_tracks")]
    public class ReleaseTrack
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
        [Column("recording_id", Order = 3)]
        public int RecordingId { get; set; }
        public Recording Recording { get; set; }

        [Required]
        [Column("track_number", Order = 4)]
        public int TrackNumber { get; set; }
    }
}
