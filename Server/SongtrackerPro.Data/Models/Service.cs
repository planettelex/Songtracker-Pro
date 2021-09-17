using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    [Table("services")]
    public class Service
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("name", Order = 2)]
        public string Name { get; set; }

        [Required]
        [Column("type", Order = 3)]
        public ServiceType Type { get; set; }
    }
}
