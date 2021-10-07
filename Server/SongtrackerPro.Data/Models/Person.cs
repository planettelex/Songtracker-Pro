using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    public class Person : LegalEntity
    {
        public Person()
        {
            EntityType = LegalEntityType.Person;
        }

        [Column("first_name", Order = 8)]
        public string FirstName { get; set; }

        [Column("middle_name", Order = 9)]
        public string MiddleName { get; set; }

        [Column("last_name", Order = 10)]
        public string LastName { get; set; }

        [Column("name_suffix", Order = 11)]
        [MaxLength(5)]
        public string NameSuffix { get; set; }

        [NotMapped]
        public string FirstAndLastName => $"{FirstName} {LastName}";
    }
}
