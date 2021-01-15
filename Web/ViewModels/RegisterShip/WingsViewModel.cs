using System.Collections.Generic;
using Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels.RegisterShip
{
    public class WingsViewModel
    {
        public IEnumerable<WingViewModel> AvailableWings { get; set; }
        public List<WingViewModel> SelectedWings { get; set; }
        
        [HiddenInput]
        public Hull Hull { get; set; }
        
        [HiddenInput]
        public Engine Engine { get; set; }
    }
}
