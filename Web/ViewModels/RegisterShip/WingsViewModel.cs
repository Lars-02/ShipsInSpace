using System.Collections.Generic;
using System.Linq;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels.RegisterShip
{
    public class WingsViewModel
    {
        public IEnumerable<Wing> AvailableWings { get; set; }

        public List<int> SelectedWings { get; set; }
        
        [HiddenInput]
        public int HullId { get; set; }
        
        [HiddenInput]
        public int EngineId { get; set; }
        
        public IEnumerable<SelectListItem> GetSelectableAvailableWings()
        {
            return AvailableWings.Select(w => new SelectListItem {Value = w.Id.ToString(), Text = w.Name});
        }
    }
}
