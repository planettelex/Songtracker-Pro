using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("legal_entity_clients")]
    public class LegalEntityClient
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
        [Column("client_legal_entity_id", Order = 3)]
        public int ClientId { get; set; }
        public LegalEntity Client { get; set; }
    }
}
