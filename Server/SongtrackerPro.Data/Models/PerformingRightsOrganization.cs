using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SongtrackerPro.Data.Models
{
    [Table("performing_rights_organizations")]
    public class PerformingRightsOrganization
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("name", Order = 2)]
        public string Name { get; set; }

        [Required]
        [Column("country_id", Order = 3)]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
