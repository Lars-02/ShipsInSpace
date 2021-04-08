using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.RegisterShip
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(9, ErrorMessage = @"A {0} has to be {1} characters.")]
        [RegularExpression(@"^[\dA-Z]{2}-{1}[\dA-Z]{3}-{1}[\dA-Z]{2}$",
            ErrorMessage = @"A {0} is in this format: 12-ABC-34. And can only use letters and numbers.")]
        [DisplayName("License plate")]
        public string LicensePlate { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = @"A {0} can be a maximum of {1} characters.")]
        [MaxLength(32, ErrorMessage = @"A {0} can be a maximum of {1} characters.")]
        [DisplayName("Code")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).*$",
            ErrorMessage = @"The code has to follow these rules:<br>
                                • At least one lowercase letter<br>
                                • At least one capital letter<br>
                                • Between 8 and 32 characters<br>
                                • At least one number")]
        public string Code { get; set; }
    }
}