using System.ComponentModel.DataAnnotations.Schema;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    public class DigitalMedia : StorageItem
    {
        [Column("media_category", Order = 12)]
        public DigitalMediaCategory? MediaCategory { get; set; }

        [Column("is_compressed", Order = 13)]
        public bool? IsCompressed { get; set; }
    }
}
