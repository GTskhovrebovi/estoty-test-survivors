using Gameplay.WeaponSystem.WeaponActions;
using UnityEngine;

namespace Gameplay.WeaponSystem
{
    public class RangedWeapon : Weapon
    {
        [SerializeField] private WeaponAction shootAction;

        protected override void Use()
        {
            if (Target == null) return;
            shootAction.TryExecute(new WeaponActionExecutionData(Owner, Owner.CharacterStats, Owner.Team, this),
                Context);
            base.Use();
        }
    }
}