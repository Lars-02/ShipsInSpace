using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(9, ErrorMessage = @"Een {0} moet {1} characters zijn.")]
        [RegularExpression(@"^[\dA-Z]{2}-{1}[\dA-Z]{3}-{1}[\dA-Z]{2}$", ErrorMessage = @"Het {0} moet dit formaat hebben: 12-ABC-34. En mag alleen uit cijfers en letters bestaan.")]
        [DisplayName("Kenteken")]
        public string LicensePlate { get; set; }
        
        [Required]
        [MinLength(8, ErrorMessage = @"Een {0} moet minimaal {1} characters zijn.")]
        [MaxLength(32, ErrorMessage = @"Een {0} mag maximaal {1} characters zijn.")]
        [DisplayName("Code")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).*$",
            ErrorMessage = @"Het wachtwoord moet aan de volgende eisen voldoen:<br>
                                • Minimaal één kleine letter<br>
                                • Minimaal één grote letter<br>
                                • Tussen de 8 en 32 letters<br>
                                • Minimaal één getal")]
        public string Code { get; set; }
    }
}