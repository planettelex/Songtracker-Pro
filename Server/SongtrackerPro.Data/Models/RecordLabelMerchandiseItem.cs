using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    public class RecordLabelMerchandiseItem : MerchandiseItem
    {
        [JsonIgnore]
        [Column("record_label_id", Order = 8)]
        public int? RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }
    }
}
