using System.ComponentModel.DataAnnotations.Schema;

namespace SongtrackerPro.Data.Models
{
    public class PublicationMerchandiseProduct : MerchandiseProduct
    {
        [Column("issue_number", Order = 10)]
        public string IssueNumber { get; set; }
    }
}
