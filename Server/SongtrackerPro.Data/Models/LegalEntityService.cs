using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SongtrackerPro.Data.Models
{
    [Table("legal_entity_services")]
    public class LegalEntityService
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("legal_entity_id", Order = 2)]
        public int LegalEntityId { get; set; }
        public LegalEntity LegalEntity { get; set; }

        [Required]
        [Column("service_id", Order = 3)]
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
