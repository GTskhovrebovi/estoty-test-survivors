using System;
using UnityEngine;

namespace Gameplay.WeaponSystem
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Weapon Data", menuName = "Gameplay/Weapon Data", order = 1)]
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField] public string WeaponName { get; set; }
        [field: SerializeField, Multiline] public string Description { get; set; }
        [field: SerializeField] public Weapon Weapon { get; set; }
        [field: SerializeField] public Variable CooldownVariable { get; set; }
    }
}