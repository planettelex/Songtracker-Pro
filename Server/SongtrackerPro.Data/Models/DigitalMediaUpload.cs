using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("digital_media_uploads")]
    public class DigitalMediaUpload
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("storage_item_id", Order = 2)]
        public Guid DigitalMediaId { get; set; }
        [JsonIgnore]
        public DigitalMedia DigitalMedia { get; set; }

        [Required]
        [Column("uploaded_on", Order = 3)]
        public DateTime UploadedOn { get; set; }

        [Required]
        [Column("uploaded_by_user_id", Order = 4)]
        public int UploadedByUserId { get; set; }
        public User UploadedByUser { get; set; }
    }
}
