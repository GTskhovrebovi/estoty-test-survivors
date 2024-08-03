using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    [CreateAssetMenu(fileName = "Apply Modifier", menuName = "Weapon Actions/Apply Modifier", order = 1)]
    public class ApplyModifier : WeaponActionOnCharacter
    {
        [SerializeField] protected ModifierData modifierData;
        [SerializeField] private bool bindToWeapon;

        public override void Execute(WeaponActionExecutionData data, Character character)
        {
            var appliedModifier = character.ModifierHolder.ApplyModifier(modifierData, data.Owner, data.CharacterStats, data.Team, data.Weapon);
            if (bindToWeapon)
                data.Weapon.BindModifier(appliedModifier);
        }
    }
}