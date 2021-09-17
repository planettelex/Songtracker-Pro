using System.ComponentModel.DataAnnotations.Schema;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    public class Document : StorageItem
    {
        [Column("document_type", Order = 14)]
        public DocumentType? DocumentType { get; set; }
        
        [Column("version", Order = 15)]
        public int? Version { get; set; }
    }
}
