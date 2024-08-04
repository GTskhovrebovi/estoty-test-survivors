using System;
using Zenject;
using Object = UnityEngine.Object;

namespace Gameplay.WeaponSystem
{
    [Serializable]
    public class WeaponSlot
    {
        public Weapon Weapon { get; private set; }
        private readonly Character _character;
        private readonly WeaponUser _weaponUser;
        public bool IsEmpty => Weapon == null;

        private Weapon.Factory _weaponFactory;

        public WeaponSlot(Weapon.Factory weaponFactory, Character character, WeaponUser weaponUser)
        {
            _weaponFactory = weaponFactory;
            _character = character;
            _weaponUser = weaponUser;
        }

        public void EquipWeapon(WeaponData weaponData)
        {
            if (Weapon != null)
            {
                return;
            }

            Weapon = _weaponFactory.Create(weaponData.Weapon.gameObject);
            Weapon.transform.SetParent(_weaponUser.WeaponContainer);
            Weapon.Initialize(_character, _weaponUser, weaponData);
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

        public class Factory : PlaceholderFactory<Character, WeaponUser, WeaponSlot>
        {
        }
    }
}