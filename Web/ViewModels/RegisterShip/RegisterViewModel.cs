using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Data.Model;

namespace Web.ViewModels.RegisterShip
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(9, ErrorMessage = @"A {0} has to be {1} characters.")]
        [RegularExpression(@"^(?:[A-Za-z]{2}|\d{2})-(?:[A-Za-z]{3}|\d{3})-(?:[A-Za-z]{2}|\d{2})$",
            ErrorMessage = @"A {0} is in this format: 12-ABC-34. And can only use letters and numbers.")]
        [DisplayName("License plate")]
        public string LicensePlate { get; set; }

        [Required(ErrorMessage = "This is not a valid option")]
        [DisplayName("License2 type")]
        [Range(0, 3)]
        public int LicenceId { get; set; } = -1;
        
        public Licence License { get; set; }
        public string SecretCode { get; set; }
    }
}