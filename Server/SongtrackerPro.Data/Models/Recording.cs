using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("recordings")]
    public class Recording
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("title", Order = 2)]
        public string Title { get; set; }

        [JsonIgnore]
        [Required]
        [Column("artist_id", Order = 2)]
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        [JsonIgnore]
        [Required]
        [Column("record_label_id", Order = 2)]
        public int RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }

        [JsonIgnore]
        [Required]
        [Column("composition_id", Order = 3)]
        public int CompositionId { get; set; }
        public Composition Composition { get; set; }

        [Column("isrc", Order = 4)]
        public string Isrc { get; set; }

        [Required]
        [Column("seconds_long", Order = 5)]
        public int SecondsLong { get; set; }

        [Required]
        [Column("is_live", Order = 6)]
        public bool IsLive { get; set; }

        [Required]
        [Column("is_cover", Order = 7)]
        public bool IsCover { get; set; }

        [Required]
        [Column("is_remix", Order = 8)]
        public bool IsRemix { get; set; }

        [JsonIgnore]
        [Column("original_recording_id", Order = 9)]
        public int? OriginalRecordingId { get; set; }
        public Recording OriginalRecording { get; set; }
    }
}
