using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SongtrackerPro.Data.Models
{
    [Table("platform_services")]
    public class PlatformService
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("platform_id", Order = 2)]
        public int PlatformId { get; set; }
        public Platform Platform { get; set; }

        [Required]
        [Column("service_id", Order = 3)]
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
