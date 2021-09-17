using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("composition_authors")]
    public class CompositionAuthor
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("composition_id", Order = 2)]
        public int CompositionId { get; set; }
        [JsonIgnore]
        public Composition Composition { get; set; }

        [JsonIgnore]
        [Required]
        [Column("person_id", Order = 3)]
        public int PersonId { get; set; }
        public Person Author { get; set; }

        [Column("ownership_percentage", Order = 4)]
        public decimal? OwnershipPercentage { get; set; }
    }
}
