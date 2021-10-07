using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    public class MerchandiseAsset : StorageItem
    {
        [JsonIgnore]
        [Column("merchandise_item_id", Order = 32)]
        public int? MerchandiseItemId { get; set; }
        public MerchandiseItem MerchandiseItem { get; set; }

        [JsonIgnore]
        [Column("product_id", Order = 33)]
        public int? ProductId { get; set; }
        public MerchandiseProduct Product { get; set; }
    }
}
