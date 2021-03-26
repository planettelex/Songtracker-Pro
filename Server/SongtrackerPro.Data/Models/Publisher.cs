using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Attributes;

namespace SongtrackerPro.Data.Models
{
    [Table("publishers")]
    public class Publisher
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("name", Order = 2)]
        public string Name { get; set; }

        [Encrypted]
        [Column("tax_id", Order = 3)]
        public string TaxId { get; set; }

        [Required]
        [Column("email", Order = 4)]
        public string Email { get; set; }

        [Column("phone", Order = 5)]
        [MaxLength(20)]
        public string Phone { get; set; }

        [JsonIgnore]
        [Column("address_id", Order = 6)]
        public int? AddressId { get; set; }
        public Address Address { get; set; }

        [JsonIgnore]
        [Column("performing_rights_organization_id", Order = 7)]
        public int? PerformingRightsOrganizationId { get; set; }
        public PerformingRightsOrganization PerformingRightsOrganization { get; set; }

        [Encrypted]
        [Column("performing_rights_organization_publisher_number", Order = 8)]
        public string PerformingRightsOrganizationPublisherNumber { get; set; }
    }
}
