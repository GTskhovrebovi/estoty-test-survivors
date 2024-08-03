using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Gameplay
{
    public class WeaponUser : MonoBehaviour
    {
        [SerializeField] private Transform weaponContainer;
        [SerializeField] private StatType numberOfWeaponsStatType;
        [SerializeField] private float weaponDistance;
        private readonly List<WeaponSlot> _weaponSlots = new();
        public Character Character { get; private set; }
        public Transform WeaponContainer => weaponContainer;
        public  List<WeaponSlot> WeaponSlots => _weaponSlots;
        public bool WeaponSlotsFull => _weaponSlots.All(i => !i.IsEmpty);
        
        private void Awake()
        {
            Character = GetComponent<Character>();
        }
    
        public void Initialize(List<WeaponData> startingWeapons)
        {
            _weaponSlots.Clear();
            var numberOfWeaponSlots = Mathf.FloorToInt(Character.CharacterStats.GetStat(numberOfWeaponsStatType).Value);

            for (var i = 0; i < numberOfWeaponSlots; i++)
            {
                _weaponSlots.Add(new WeaponSlot(Character, this));
            }
        
            foreach (var weaponData in startingWeapons)
            {
                EquipWeapon(weaponData);
            }
        }
    
        public void EquipWeaponInFirstEmptySlot(WeaponData weaponData)
        {
            var firstEmptySlot = _weaponSlots.FirstOrDefault(i => i.IsEmpty);
            if (firstEmptySlot == null) return;
            EquipWeapon(weaponData, firstEmptySlot);
        }
    
        public void EquipWeapon(WeaponData weaponData, WeaponSlot slot)
        {
            slot.EquipWeapon(weaponData);
            ArrangeWeaponsOnCircle();
        }

        public void EquipWeapon(WeaponData weaponData)
        {
            if (weaponData == null) return;
            if (WeaponSlotsFull) return;
            
            var emptyWeaponSlot = _weaponSlots.First(i => i.IsEmpty);
            emptyWeaponSlot.EquipWeapon(weaponData);
            ArrangeWeaponsOnCircle();
        }
    
        public void UnequipWeapon(WeaponData weaponData)
        {
            if (_weaponSlots.TryGetFirst(i => !i.IsEmpty && i.Weapon.WeaponData == weaponData, out var weaponSlot))
            {
                weaponSlot.UnequipWeapon();
            }
            else
            {
                Debug.Log("Weapon Not Equipped:" + weaponData.WeaponName);
            }
        }
    
        private void ArrangeWeaponsOnCircle()
        {
            var weaponSlots = _weaponSlots.Where(weaponSlot => !weaponSlot.IsEmpty).ToList();
            var angleStep = 360f / weaponSlots.Count();
            float currentAngle = 0;

            for (int i = 0; i < weaponSlots.Count(); i++)
            {
                var child = weaponSlots[i].Weapon;
                var radianAngle = currentAngle * Mathf.Deg2Rad;
                var x = Mathf.Cos(radianAngle) * weaponDistance;
                var y = Mathf.Sin(radianAngle) * weaponDistance;

                child.transform.localPosition = new Vector3(x, y, 0);
                currentAngle += angleStep;
            }
        }
    }
}