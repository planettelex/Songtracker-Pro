using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("logins")]
    public class Login
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Column("user_id", Order = 2)]
        public int? UserId { get; set; }

        [NotMapped]
        public string AuthenticationId { get; set; }

        [Required]
        [Column("authentication_token", Order = 3)]
        public string AuthenticationToken { get; set; }

        [Required]
        [Column("login_at", Order = 4)]
        public DateTime LoginAt { get; set; }

        [Column("logout_at", Order = 5)]
        public DateTime? LogoutAt { get; set; }

        public User User { get; set; }
    }
}
