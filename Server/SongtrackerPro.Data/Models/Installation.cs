using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SongtrackerPro.Data.Models
{
    [Table("installation")]
    public class Installation
    {
        [Key]
        [Required]
        [Column("uuid", Order = 1)]
        public Guid Uuid { get; set; }

        [Required]
        [Column("version", Order = 2)]
        [MaxLength(25)]
        public string Version { get; set; }

        [Required]
        [Column("name", Order = 3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Column("tagline", Order = 4)]
        [MaxLength(200)]
        public string Tagline { get; set; }

        [NotMapped]
        public string OAuthId { get; set; }

        [NotMapped]
        public string OAuthConsole { get; set; }

        [NotMapped]
        public string HostingConsole { get; set; }

        [NotMapped]
        public string ApiHostingConsole { get; set; }

        [NotMapped]
        public string DatabaseConsole { get; set; }

        [NotMapped]
        public string DatabaseName { get; set; }
    }
}
