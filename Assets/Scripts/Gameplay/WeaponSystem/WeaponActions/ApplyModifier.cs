using System;
using Gameplay.ModifierSystem;
using UnityEngine;
using Zenject;

namespace Gameplay.WeaponSystem.WeaponActions
{
    [Serializable]
    [CreateAssetMenu(fileName = "Apply Modifier", menuName = "Gameplay/Weapon Actions/Apply Modifier", order = 1)]
    public class ApplyModifier : WeaponActionOnCharacter
    {
        [SerializeField] protected ModifierData modifierData;
        [SerializeField] private bool bindToWeapon;

        protected override void Execute(WeaponActionExecutionData data, DiContainer container, Character character)
        {
            var appliedModifier = character.ModifierHolder.ApplyModifier(modifierData, data.Owner, data.CharacterStats,
                data.Team, data.Weapon);
            if (bindToWeapon)
                data.Weapon.BindModifier(appliedModifier);
        }
    }
}