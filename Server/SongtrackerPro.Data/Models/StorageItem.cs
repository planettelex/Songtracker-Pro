using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("storage_items")]
    public class StorageItem
    {
        [Key]
        [Required]
        [Column("uuid", Order = 1)]
        public Guid Uuid { get; set; }

        [JsonIgnore]
        [Column("publisher_id", Order = 2)]
        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [JsonIgnore]
        [Column("record_label_id", Order = 3)]
        public int? RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }

        [JsonIgnore]
        [Column("artist_id", Order = 4)]
        public int? ArtistId { get; set; }
        public Artist Artist { get; set; }

        [Required]
        [Column("name", Order = 5)]
        public string Name { get; set; }

        [Required]
        [Column("file_name", Order = 6)]
        public string FileName { get; set; }

        [Required]
        [Column("folder_path", Order = 7)]
        public string FolderPath { get; set; }

        [Required]
        [Column("container", Order = 8)]
        public string Container { get; set; }

        [Required]
        [Column("created_on", Order = 9)]
        public DateTime CreatedOn { get; set; }

        [Column("updated_on", Order = 10)]
        public DateTime? UpdatedOn { get; set; }
    }
}
