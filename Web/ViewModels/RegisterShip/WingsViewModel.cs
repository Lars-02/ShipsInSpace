using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels.RegisterShip
{
    public class WingsViewModel : IValidatableObject
    {
        public IEnumerable<Wing> AvailableWings { get; set; }
        public IEnumerable<Weapon> AvailableWeapons { get; set; }

        [Required]
        public int[] SelectedWings { get; set; }
        
        [Required]
        public List<int>[] SelectedWeapons { get; set; }
        
        [HiddenInput]
        public Hull Hull { get; set; }

        [HiddenInput]
        public Engine Engine { get; set; }

        [HiddenInput]
        public int HullId { get; set; }
        
        [HiddenInput]
        public int EngineId { get; set; }
        
        public IEnumerable<SelectListItem> GetSelectableAvailableWings() => AvailableWings.Select(w => new SelectListItem {Value = w.Id.ToString(), Text = $"{w.Name} -  {w.Agility},  {w.Speed},  {w.Energy},  {w.Weight},  {w.NumberOfHardpoints}"});
        public IEnumerable<SelectListItem> GetSelectableAvailableWeapons() => AvailableWeapons.Select(w => new SelectListItem {Value = w.Id.ToString(), Text = $"{w.Name} - {w.DamageType},  {w.EnergyDrain},  {w.Weight}"});

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TotalWeight() > (int) Hull.DefaultMaximumTakeOffMass) yield return new ValidationResult("Error message goes here");
            if (true) yield return new ValidationResult(TotalWeightWeapons().ToString());
        }

        private int TotalWeight()
        {
            return Engine.Weight + TotalWeightWings() + TotalWeightWeapons();
        }

        private int TotalWeightWeapons()
        {
           return SelectedWeapons.Sum(selectedWeapons => selectedWeapons.Sum(selectedWeapon =>
                AvailableWeapons.FirstOrDefault(weapon => weapon.Id == selectedWeapon).Weight));
        }

        private int TotalWeightWings()
        {
            return 10;
        }
    }
}
