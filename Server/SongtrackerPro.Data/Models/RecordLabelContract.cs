using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    public class RecordLabelContract : Contract
    {
        [JsonIgnore]
        [Column("recording_id", Order = 30)]
        public int? RecordingId { get; set; }
        public Recording Recording { get; set; }

        [JsonIgnore]
        [Column("release_id", Order = 31)]
        public int? ReleaseId { get; set; }
        public Release Release { get; set; }
    }
}
