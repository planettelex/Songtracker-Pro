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
        [Column("artist_id", Order = 3)]
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        [JsonIgnore]
        [Required]
        [Column("record_label_id", Order = 4)]
        public int RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }

        [JsonIgnore]
        [Required]
        [Column("composition_id", Order = 5)]
        public int CompositionId { get; set; }
        public Composition Composition { get; set; }

        [JsonIgnore]
        [Column("genre_id", Order = 6)]
        public int? GenreId { get; set; }
        public Genre Genre { get; set; }

        [Column("isrc", Order = 7)]
        public string Isrc { get; set; }

        [Required]
        [Column("seconds_long", Order = 8)]
        public int SecondsLong { get; set; }

        [Required]
        [Column("is_live", Order = 9)]
        public bool IsLive { get; set; }

        [Required]
        [Column("is_cover", Order = 10)]
        public bool IsCover { get; set; }

        [Required]
        [Column("is_remix", Order = 11)]
        public bool IsRemix { get; set; }

        [JsonIgnore]
        [Column("original_recording_id", Order = 12)]
        public int? OriginalRecordingId { get; set; }
        public Recording OriginalRecording { get; set; }
    }
}
