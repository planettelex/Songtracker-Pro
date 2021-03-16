using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("addresses")]
    public class Address
    {
        [Key]
        [JsonIgnore]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("street", Order = 2)]
        public string Street { get; set; }

        [Required]
        [Column("city", Order = 3)]
        public string City { get; set; }

        [Required]
        [Column("region", Order = 4)]
        [MaxLength(3)]
        public string Region { get; set; }

        [Required]
        [Column("postal_code", Order = 5)]
        public string PostalCode { get; set; }

        [JsonIgnore]
        [Column("country_id", Order = 6)]
        public int? CountryId { get; set; }
        public Country Country { get; set; }
    }
}
