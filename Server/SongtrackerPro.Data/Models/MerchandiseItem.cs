﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("merchandise")]
    public class MerchandiseItem
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("name", Order = 2)]
        public string Name { get; set; }

        [Column("description", Order = 3)]
        public string Description { get; set; }

        [JsonIgnore]
        [Column("category_id", Order = 4)]
        public int? CategoryId { get; set; }
        public MerchandiseCategory Category { get; set; }

        [Required]
        [Column("is_promotional", Order = 5)]
        public bool IsPromotional { get; set; }

        [JsonIgnore]
        [Column("artist_id", Order = 6)]
        public int? ArtistId { get; set; }
        public Artist Artist { get; set; }
    }
}
