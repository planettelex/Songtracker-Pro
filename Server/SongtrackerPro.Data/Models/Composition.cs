using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("compositions")]
    public class Composition
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("title", Order = 2)]
        public string Title { get; set; }

        [JsonIgnore]
        [Column("publisher_id", Order = 3)]
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [JsonIgnore]
        [Column("legal_entity_id", Order = 4)]
        public int LegalEntityId { get; set; }
        public LegalEntity ExternalPublisher { get; set; }

        [Column("iswc", Order = 5)]
        public string Isbn { get; set; }

        [Column("catalog_number", Order = 6)]
        public string CatalogNumber { get; set; }
 
        [Column("copyrighted_on", Order = 7)]
        public DateTime? CopyrightedOn { get; set; }
    }
}
