using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("people")]
    public class Person
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("first_name", Order = 2)]
        public string FirstName { get; set; }

        [Column("middle_name", Order = 3)]
        public string MiddleName { get; set; }

        [Required]
        [Column("last_name", Order = 4)]
        public string LastName { get; set; }

        [Column("name_suffix", Order = 5)]
        [MaxLength(5)]
        public string NameSuffix { get; set; }

        [JsonIgnore] 
        public string FirstAndLastName => $"{FirstName} {LastName}";

        [Column("email", Order = 6)]
        public string Email { get; set; }

        [Column("phone", Order = 7)]
        [MaxLength(20)]
        public string Phone { get; set; }

        [JsonIgnore]
        [Column("address_id", Order = 8)]
        public int? AddressId { get; set; }
        public Address Address { get; set; }
    }
}
