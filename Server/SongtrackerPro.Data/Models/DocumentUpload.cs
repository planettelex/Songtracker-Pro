using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("document_uploads")]
    public class DocumentUpload
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("storage_item_id", Order = 2)]
        public Guid DocumentId { get; set; }
        [JsonIgnore]
        public Document Document { get; set; }

        [Required]
        [Column("uploaded_on", Order = 3)]
        public DateTime UploadedOn { get; set; }

        [Required]
        [Column("uploaded_by_user_id", Order = 4)]
        public int UploadedByUserId { get; set; }
        public User UploadedByUser { get; set; }

        [Column("from_version", Order = 5)]
        public int? FromVersion { get; set; }

        [Required]
        [Column("to_version", Order = 6)]
        public int ToVersion { get; set; }
    }
}
