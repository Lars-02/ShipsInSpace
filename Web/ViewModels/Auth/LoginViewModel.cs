using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Auth
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Licence plate*")]
        public string LicencePlate { get; set; }

        [Required]
        [DisplayName("Secret code*")]
        public string Code { get; set; }
    }
}
