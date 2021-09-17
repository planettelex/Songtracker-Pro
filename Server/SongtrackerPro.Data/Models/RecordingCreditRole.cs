using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("recording_credit_roles")]
    public class RecordingCreditRole
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("recording_credit_id", Order = 2)]
        public int RecordingCreditId { get; set; }
        [JsonIgnore]
        public RecordingCredit RecordingCredit { get; set; }

        [JsonIgnore]
        [Required]
        [Column("recording_role_id", Order = 3)]
        public int RecordingRoleId { get; set; }
        public RecordingRole RecordingRole { get; set; }
    }
}
