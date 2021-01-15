using System.Collections.Generic;

namespace Web.ViewModels.RegisterShip
{
    public class WingsViewModel
    {
        public IEnumerable<WingViewModel> AvailableWings { get; set; }
        public List<WingViewModel> SelectedWings { get; set; }
    }
}
