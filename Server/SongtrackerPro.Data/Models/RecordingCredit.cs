using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("recording_credits")]
    public class RecordingCredit
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("recording_id", Order = 2)]
        public int RecordingId { get; set; }
        [JsonIgnore]
        public Recording Recording { get; set; }

        [JsonIgnore]
        [Required]
        [Column("person_id", Order = 3)]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        [Required]
        [Column("is_featured", Order = 4)]
        public bool IsFeatured { get; set; }

        [Column("ownership_percentage", Order = 5)]
        public decimal? OwnershipPercentage { get; set; }

        [JsonIgnore]
        public List<RecordingCreditRole> RecordingCreditRoles { get; set; }

        [NotMapped]
        public List<RecordingRole> Roles
        {
            get
            {
                if (_roles == null && RecordingCreditRoles != null)
                {
                    _roles = new List<RecordingRole>();
                    _roles.AddRange(RecordingCreditRoles.Select(recordingCreditRole => recordingCreditRole.RecordingRole));
                }

                return _roles;
            }
            set => _roles = value;
        }
        private List<RecordingRole> _roles;
    }
}
