using System.ComponentModel.DataAnnotations.Schema;

namespace SongtrackerPro.Data.Models
{
    [NotMapped]
    public class Login
    {
        public string AuthenticationId { get; set; }

        public string AuthenticationToken { get; set; }
    }
}
