using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("License plate")]
        public string LicensePlate { get; set; }

        [Required]
        [DisplayName("Secret code")]
        public string SecretCode { get; set; }
    }
}