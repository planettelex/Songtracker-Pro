using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SongtrackerPro.Data.Models
{
    [Table("artist_managers")]
    public class ArtistManager
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("artist_id", Order = 2)]
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        [Required]
        [Column("person_id", Order = 3)]
        public int PersonId { get; set; }
        public Person Member { get; set; }

        [Required]
        [Column("is_active", Order = 4)]
        public bool IsActive { get; set; }

        [Required]
        [Column("started_on", Order = 5)]
        public DateTime StartedOn { get; set; }

        [Column("ended_on", Order = 6)]
        public DateTime? EndedOn { get; set; }
    }
}
