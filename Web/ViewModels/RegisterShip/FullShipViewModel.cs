using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels.RegisterShip
{
    public class FullShipViewModel
    {
        [HiddenInput] public double MaximumTakeoffMass { get; set; }
        [HiddenInput] public int HullId { get; set; }
        [HiddenInput] public int EngineId { get; set; }
        [HiddenInput] public int[] SelectedWings { get; set; }
        [HiddenInput] public List<int>[] SelectedWeapons { get; set; }
    }
}