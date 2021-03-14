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

        [Required]
        [Column("oauth_id", Order = 5)]
        public string OAuthId { get; set; }

        [Column("oauth_console_url", Order = 6)]
        public string OAuthConsoleUrl { get; set; }

        [Column("hosting_console_url", Order = 7)]
        public string HostingConsoleUrl { get; set; }

        [Column("api_hosting_console_url", Order = 8)]
        public string ApiHostingConsoleUrl { get; set; }

        [Column("database_console_url", Order = 9)]
        public string DatabaseConsoleUrl { get; set; }

        [NotMapped]
        public string DatabaseName { get; set; }
    }
}
