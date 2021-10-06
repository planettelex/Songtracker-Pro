﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    [Table("releases")]
    public class Release
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("title", Order = 2)]
        public string Title { get; set; }

        [JsonIgnore]
        [Column("artist_id", Order = 3)]
        public int? ArtistId { get; set; }
        public Artist Artist { get; set; }

        [JsonIgnore]
        [Required]
        [Column("record_label_id", Order = 4)]
        public int RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }

        [JsonIgnore]
        [Column("genre_id", Order = 5)]
        public int? GenreId { get; set; }
        public Genre Genre { get; set; }

        [Required]
        [Column("type", Order = 6)]
        public ReleaseType Type { get; set; }

        [Column("catalog_number", Order = 7)]
        public string CatalogNumber { get; set; }
    }
}