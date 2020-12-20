using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Data.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels
{
    public class RegisterShipViewModel : IValidatableObject
    {
        [Required]
        public int SelectedHull { get; set; }
        
        [DisplayName("Hull")] 
        public SelectList Hulls { get; set; }
        
        [Required]
        public int SelectedEngine { get; set; }
        
        [DisplayName("Engine")] 
        public SelectList Engines { get; set; }

        [Range(0, 100, ErrorMessage = "The {0} must be between {1} and {2}.")]
        [DisplayName("Number of wings")]
        public byte NumberOfWings { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NumberOfWings%2 != 0)
                yield return new ValidationResult("A ship has an even number of wings.",
                    new[] {nameof(NumberOfWings)});
        }
    }
}