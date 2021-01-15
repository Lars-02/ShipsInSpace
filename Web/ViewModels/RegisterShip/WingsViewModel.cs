using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels.RegisterShip
{
    public class WingsViewModel
    {
        public IEnumerable<WingViewModel> AvailableWings { get; set; }
        public List<WingViewModel> SelectedWings { get; set; }
        
        [HiddenInput]
        public int HullId { get; set; }
        
        [HiddenInput]
        public int EngineId { get; set; }
    }
}
