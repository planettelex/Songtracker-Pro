using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    [Table("user_invitations")]
    public class UserInvitation
    {
        [Key]
        [Required]
        [Column("uuid", Order = 1)]
        public Guid Uuid { get; set; }

        [Required]
        [Column("invited_by_user_id", Order = 2)]
        public int InvitedByUserId { get; set; }
        public User InvitedByUser { get; set; }

        [Required]
        [Column("name", Order = 3)]
        public string Name { get; set; }

        [Required]
        [Column("email", Order = 4)]
        public string Email { get; set; }

        [Required]
        [Column("type", Order = 5)]
        public UserType Type { get; set; }

        [JsonIgnore]
        [Column("publisher_id", Order = 6)]
        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [JsonIgnore]
        [Column("record_label_id", Order = 7)]
        public int? RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }

        [JsonIgnore]
        [Column("artist_id", Order = 8)]
        public int? ArtistId { get; set; }
        public Artist Artist { get; set; }

        [Required]
        [Column("sent_on", Order = 9)]
        public DateTime SentOn { get; set; }

        [Column("accepted_on", Order = 10)]
        public DateTime? AcceptedOn { get; set; }

        [JsonIgnore]
        [Column("created_user_id", Order = 11)]
        public int? CreatedUserId { get; set; }
        public User CreatedUser { get; set; }

        [JsonIgnore]
        [NotMapped]
        public string AcceptLink { get; set; }

        [JsonIgnore]
        [NotMapped]
        public string LoginLink { get; set; }
    }
}
