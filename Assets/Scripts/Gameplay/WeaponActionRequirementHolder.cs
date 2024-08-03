using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class WeaponActionRequirementHolder : MonoBehaviour
    {
        private readonly List<WeaponActionRequirement> _appliedRequirements = new();
        
        public void AddRequirement(WeaponActionRequirement requirement)
        {
            if (requirement == null) return;
            if (_appliedRequirements.Contains(requirement)) return;
            
            _appliedRequirements.Add(requirement);
        }

        public void RemoveRequirement(WeaponActionRequirement requirement)
        {
            if (_appliedRequirements.Contains(requirement))
                _appliedRequirements.Remove(requirement);
        }

        public void AddRequirements(List<WeaponActionRequirement> requirements)
        {
            if (requirements == null) return;
            foreach (var requirement in requirements)
            {
                AddRequirement(requirement);
            }
        }
        
        public void RemoveRequirements(List<WeaponActionRequirement> requirements)
        {
            if (requirements == null) return;
            foreach (var requirement in requirements)
            {
                RemoveRequirement(requirement);
            }
        }
        
        public bool HasRequirement(WeaponActionRequirement requirements)
        {
            return _appliedRequirements.Contains(requirements);
        }
    }
}