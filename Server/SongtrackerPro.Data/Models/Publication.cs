using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("publications")]
    public class Publication
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
        [Column("publisher_id", Order = 3)]
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [Column("isbn", Order = 4)]
        public string Isbn { get; set; }

        [Column("catalog_number", Order = 5)]
        public string CatalogNumber { get; set; }
 
        [Column("copyrighted_on", Order = 6)]
        public DateTime? CopyrightedOn { get; set; }
    }
}
