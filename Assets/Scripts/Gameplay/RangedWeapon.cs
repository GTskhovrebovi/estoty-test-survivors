using UnityEngine;

namespace Gameplay
{
    public class RangedWeapon : Weapon
    {
        [SerializeField] private WeaponAction shootAction;
        protected override void Use()
        {
            base.Use();
            shootAction.Execute(new WeaponActionExecutionData(Owner, Owner.CharacterStats, Owner.Team, this));
        }
    }
}