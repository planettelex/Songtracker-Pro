using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("user_accounts")]
    public class UserAccount
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("user_id", Order = 2)]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        [Required]
        [Column("platform_id", Order = 3)]
        public int PlatformId { get; set; }
        public Platform Platform { get; set; }

        [Required]
        [Column("username", Order = 4)]
        public string Username { get; set; }

        [Required]
        [Column("is_preferred", Order = 5)]
        public bool IsPreferred { get; set; }
    }
}
