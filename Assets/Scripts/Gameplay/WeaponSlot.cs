using System;
using Object = UnityEngine.Object;

namespace Gameplay
{
    public class WeaponSlot
    {
        public Weapon Weapon { get; private set; }
        private readonly Character _character;
        private readonly WeaponUser _weaponUser;
        public bool IsEmpty => Weapon == null;
        
        public WeaponSlot(Character character, WeaponUser weaponUser)
        {
            _character = character;
            _weaponUser = weaponUser;
        }
        
        public Weapon EquipWeapon(WeaponData weaponData)
        {
            if (Weapon != null) { return null; }
        
            Weapon = Object.Instantiate(weaponData.Weapon, _weaponUser.WeaponContainer);
            Weapon.Initialize(_character, _weaponUser, weaponData);
            return Weapon;
        }
        
        public void UnequipWeapon()
        {
            if (IsEmpty) return;
            Object.Destroy(Weapon);
            Weapon = null;
        }

        public void ReplaceWeapon(WeaponData weaponData)
        {
            UnequipWeapon();
            EquipWeapon(weaponData);
        }
    }
}