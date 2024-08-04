using System.Collections.Generic;
using Gameplay.ModifierSystem;
using Gameplay.WeaponSystem;
using UnityEngine;

namespace Gameplay.UpgradeSystem
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "Gameplay/Upgrade", order = 1)]
    public class Upgrade : ScriptableObject
    {
        [field: SerializeField] public string UpgradeName { get; set; }
        [field: SerializeField, Multiline] public string UpgradeDescription { get; set; }
        [field: SerializeField] public List<StatModifier> StatModifiers { get; set; }
        [field: SerializeField] public List<WeaponActionRequirement> WeaponActionRequirements { get; set; }
    }
}