using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("type", Order = 2)]
        public UserType Type { get; set; }

        [Column("profile_image_url", Order = 3)]
        public string ProfileImageUrl { get; set; }

        [Required]
        [Column("authentication_id", Order = 4)]
        public string AuthenticationId { get; set; }

        [Required]
        [Column("authentication_token", Order = 5)]
        public string AuthenticationToken { get; set; }

        [Column("last_login", Order = 6)]
        public DateTime? LastLogin { get; set; }

        [JsonIgnore]
        [Column("person_id", Order = 7)]
        public int? PersonId { get; set; }
        public Person Person { get; set; }

        [Column("social_security_number", Order = 8)]
        public string SocialSecurityNumber { get; set; }

        [JsonIgnore]
        [Column("publisher_id", Order = 9)]
        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [JsonIgnore]
        [Column("record_label_id", Order = 10)]
        public int? RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }

        [JsonIgnore]
        [Column("performing_rights_organization_id", Order = 11)]
        public int? PerformingRightsOrganizationId { get; set; }
        public PerformingRightsOrganization PerformingRightsOrganization { get; set; }

        [Column("performing_rights_organization_member_number", Order = 12)]
        public string PerformingRightsOrganizationMemberNumber { get; set; }

        [Column("sound_exchange_account_number", Order = 13)]
        public string SoundExchangeAccountNumber { get; set; }
    }
}
