using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon Data", order = 1)]
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public string WeaponName { get; set; }
        [field: SerializeField, Multiline] public string Description { get; set; }
        [field: SerializeField] public Weapon Weapon { get; set; }
        [field: SerializeField] public bool HasCast { get; set; } = true;
        [field: SerializeField] public Variable CooldownVariable { get; set; }
    }
}