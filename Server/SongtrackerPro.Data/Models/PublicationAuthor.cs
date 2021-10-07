using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("publication_authors")]
    public class PublicationAuthor
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("publication_id", Order = 2)]
        public int PublicationId { get; set; }
        [JsonIgnore]
        public Publication Publication { get; set; }

        [JsonIgnore]
        [Required]
        [Column("person_id", Order = 3)]
        public int PersonId { get; set; }
        public Person Author { get; set; }

        [Required]
        [Column("ownership_percentage", Order = 4)]
        public decimal OwnershipPercentage { get; set; }
    }
}
