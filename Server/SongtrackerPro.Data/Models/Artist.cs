using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    public class Artist : Business
    {
        [Column("press_kit_url", Order = 26)]
        public string PressKitUrl { get; set; }

        [JsonIgnore]
        [Column("record_label_id", Order = 27)]
        public int? RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }
    }
}
