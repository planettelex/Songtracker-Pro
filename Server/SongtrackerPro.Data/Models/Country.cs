using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SongtrackerPro.Data.Models
{
    [Table("countries")]
    public class Country
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("name", Order = 2)]
        public string Name { get; set; }

        [Required]
        [Column("iso_code", Order = 3)]
        [MaxLength(3)]
        public string IsoCode { get; set; }
    }
}
