using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column("email", Order = 3)]
        public string Email { get; set; }

        [Required]
        [Column("type", Order = 4)]
        public UserType Type { get; set; }

        [Column("publisher_id", Order = 5)]
        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [Column("record_label_id", Order = 6)]
        public int? RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }

        [Column("artist_id", Order = 7)]
        public int? ArtistId { get; set; }
        public Artist Artist { get; set; }

        [Required]
        [Column("sent_on", Order = 8)]
        public DateTime SentOn { get; set; }

        [Column("accepted_on", Order = 9)]
        public DateTime? AcceptedOn { get; set; }

        [Column("created_user_id", Order = 2)]
        public int? CreatedUserId { get; set; }
        public User CreatedUser { get; set; }
    }
}
