using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels.RegisterShip
{
    public class RegisterShipViewModel
    {
        [Required] [DisplayName("Hull")] public int SelectedHull { get; set; }

        public List<SelectListItem> Hulls { get; set; }

        [Required] [DisplayName("Engine")] public int SelectedEngine { get; set; }

        public List<SelectListItem> Engines { get; set; }

        [Range(2, 100, ErrorMessage = "The {0} must be between {1} and {2}.")]
        [Remote("VerifyNumberOfWings", "RegisterShip", HttpMethod = "POST",
            AdditionalFields = "__RequestVerificationToken")]
        [DisplayName("Number of wings")]
        public byte NumberOfWings { get; set; } = 2;
    }
}