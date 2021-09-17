﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("merchandise_categories")]
    public class MerchandiseCategory
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("name", Order = 2)]
        public string Name { get; set; }

        [Column("description", Order = 3)]
        public string Description { get; set; }

        [JsonIgnore]
        [Required]
        [Column("parent_category_id", Order = 4)]
        public int ParentCategoryId { get; set; }
        public MerchandiseCategory ParentCategory { get; set; }
    }
}
