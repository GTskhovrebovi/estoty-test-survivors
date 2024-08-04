using UnityEngine;

namespace Gameplay
{
    public class RangedWeapon : Weapon
    {
        [SerializeField] private WeaponAction shootAction;
        protected override void Use()
        {
            if (Target == null) return;
            shootAction.TryExecute(new WeaponActionExecutionData(Owner, Owner.CharacterStats, Owner.Team, this), Container);
            base.Use();
        }
    }
}