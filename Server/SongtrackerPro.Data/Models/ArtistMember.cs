using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("artist_members")]
    public class ArtistMember
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("artist_id", Order = 2)]
        public int ArtistId { get; set; }
        [JsonIgnore]
        public Artist Artist { get; set; }

        [JsonIgnore]
        [Required]
        [Column("person_id", Order = 3)]
        public int PersonId { get; set; }
        public Person Member { get; set; }

        [Required]
        [Column("is_active", Order = 4)]
        public bool IsActive { get; set; }

        [Required]
        [Column("started_on", Order = 5)]
        public DateTime StartedOn { get; set; }

        [Column("ended_on", Order = 6)]
        public DateTime? EndedOn { get; set; }

        [JsonIgnore]
        public List<ArtistMemberRole> ArtistMemberRoles { get; set; }

        [NotMapped]
        public List<RecordingRole> Roles
        {
            get
            {
                if (_roles == null && ArtistMemberRoles != null)
                {
                    _roles = new List<RecordingRole>();
                    _roles.AddRange(ArtistMemberRoles.Select(artistMemberRole => artistMemberRole.RecordingRole));
                }

                return _roles;
            }
            set => _roles = value;
        }
        private List<RecordingRole> _roles;
    }
}
