using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("merchandise_products")]
    public class MerchandiseProduct
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("merchandise_item_id", Order = 2)]
        public int MerchandiseItemId { get; set; }
        public MerchandiseItem MerchandiseItem { get; set; }

        [Required]
        [Column("name", Order = 3)]
        public string Name { get; set; }

        [Column("description", Order = 4)]
        public string Description { get; set; }

        [Column("sku", Order = 5)]
        public string Sku { get; set; }

        [Column("upc", Order = 6)]
        public string Upc { get; set; }

        [Column("color", Order = 7)]
        public string Color { get; set; }

        [Column("color_name", Order = 8)]
        public string ColorName { get; set; }

        [Column("size", Order = 9)]
        public string Size { get; set; }
    }
}
