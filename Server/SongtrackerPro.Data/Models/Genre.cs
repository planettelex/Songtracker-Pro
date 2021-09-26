using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("genres")]
    public class Genre
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("name", Order = 2)]
        public string Name { get; set; }

        [JsonIgnore]
        [Column("parent_genre_id", Order = 3)]
        public int? ParentGenreId { get; set; }
        public Genre ParentGenre { get; set; }
    }
}
