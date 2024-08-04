using Gameplay.WeaponSystem;
using UnityEngine;

namespace Gameplay
{
    public class WeaponPickUp : PickUp
    {
        [field: SerializeField] public WeaponData WeaponData { get; private set; }

        protected override void Consume(Character character)
        {
            base.Consume(character);
            if (character.TryGetComponent<WeaponUser>(out var weaponUser))
            {
                weaponUser.EquipWeapon(WeaponData);
            }
        }
    }
}