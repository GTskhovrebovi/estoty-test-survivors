using System;
using UnityEngine;

namespace Gameplay
{
    public interface IRewardAction
    {
        public void Execute(Character character);
    }
    
    [Serializable]
    public class AddWeapon : IRewardAction
    {
        [field: SerializeField] public WeaponData WeaponData { get; private set; }

        public AddWeapon(WeaponData weaponData)
        {
            WeaponData = weaponData;
        }

        public void Execute(Character character)
        {
            character.WeaponUser.EquipWeapon(WeaponData);
        }
    }
    
    [Serializable]
    public class AddUpgrade : IRewardAction
    {
        public Upgrade Upgrade { get; private set; }

        public AddUpgrade(Upgrade upgrade)
        {
            Upgrade = upgrade;
        }

        public void Execute(Character character)
        {
            character.GetComponent<UpgradeHolder>().ApplyUpgrade(Upgrade);
        }
    }
}