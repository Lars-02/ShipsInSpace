using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels
{
    public class RegisterShipWingsViewModel
    {
        [Required]
        [DisplayName("Wings")]
        public List<int> SelectedWings { get; set; }
        
        public List<SelectListItem> Wings { get; set; }
        
        
        [HiddenInput]
        public Hull Hull { get; set; }
        
        [HiddenInput]
        public Engine Engine { get; set; }

            [Range(2, 100, ErrorMessage = "The {0} must be between {1} and {2}.")]
        [Remote(action: "VerifyNumberOfWings", controller: "RegisterShip", HttpMethod = "POST",
            AdditionalFields = "__RequestVerificationToken")]
        [DisplayName("Number of wings")]
        public byte NumberOfWings { get; set; }
    }
}