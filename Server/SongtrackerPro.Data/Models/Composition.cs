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
        [Column("outside_publisher_id", Order = 4)]
        public int? OutsidePublisherId { get; set; }
        public LegalEntity OutsidePublisher { get; set; }

        [Column("is_published", Order = 5)]
        public bool IsPublished { get; set; }

        [Column("iswc", Order = 6)]
        public string Iswc { get; set; }

        [Column("catalog_number", Order = 7)]
        public string CatalogNumber { get; set; }
 
        [Column("copyright_registered_on", Order = 8)]
        public DateTime? CopyrightRegisteredOn { get; set; }

        [Column("published_on", Order = 9)]
        public DateTime? PublishedOn { get; set; }
    }
}
