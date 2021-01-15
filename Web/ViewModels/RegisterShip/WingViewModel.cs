using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels.RegisterShip
{
    public class WingViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int Weight { get; set; }
        
        public int WeaponSlots { get; set; }

        public override string ToString() => Name;
    }
}
