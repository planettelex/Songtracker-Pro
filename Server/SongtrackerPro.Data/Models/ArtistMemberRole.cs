using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    public class ArtistMemberRole
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("artist_member_id", Order = 2)]
        public int ArtistMemberId { get; set; }
        [JsonIgnore]
        public ArtistMember ArtistMember { get; set; }

        [JsonIgnore]
        [Required]
        [Column("recording_role_id", Order = 3)]
        public int RecordingRoleId { get; set; }
        public RecordingRole RecordingRole { get; set; }
    }
}
