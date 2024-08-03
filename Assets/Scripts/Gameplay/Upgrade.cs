using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade", order = 1)]
    public class Upgrade : ScriptableObject
    {
        [field: SerializeField] public string UpgradeName { get; set; }
        [field: SerializeField, Multiline] public string UpgradeDescription { get; set; }
        [field: SerializeField] public List<StatModifier> StatModifiers { get; set; }
        [field: SerializeField] public List<WeaponActionRequirement> WeaponActionRequirements { get; set; }
    }
}