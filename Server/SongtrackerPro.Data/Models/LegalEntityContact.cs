using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("legal_entity_contacts")]
    public class LegalEntityContact
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("legal_entity_id", Order = 2)]
        public int LegalEntityId { get; set; }
        [JsonIgnore]
        public LegalEntity LegalEntity { get; set; }

        [Required]
        [Column("contact_legal_entity_id", Order = 3)]
        public int ContactId { get; set; }
        public LegalEntity Contact { get; set; }

        [Column("position", Order = 4)]
        public string Position { get; set; }
    }
}
