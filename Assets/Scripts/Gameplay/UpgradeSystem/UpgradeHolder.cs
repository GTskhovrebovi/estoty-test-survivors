using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UpgradeSystem
{
    public class UpgradeHolder : MonoBehaviour
    {
        private List<AppliedUpgrade> _appliedUpgrades = new();
        private Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        public void ApplyUpgrade(Upgrade upgrade)
        {
            var appliedUpgrade = new AppliedUpgrade(upgrade);
            _appliedUpgrades.Add(appliedUpgrade);
            _character.CharacterStats.ApplyModifiers(upgrade.StatModifiers, appliedUpgrade);
            _character.WeaponActionRequirementHolder.AddRequirements(upgrade.WeaponActionRequirements);
        }

        public void RemoveUpgrade(AppliedUpgrade appliedUpgrade)
        {
            _appliedUpgrades.Remove(appliedUpgrade);
            _character.CharacterStats.RemoveAllModifiersFromSource(appliedUpgrade);
            _character.WeaponActionRequirementHolder.RemoveRequirements(appliedUpgrade.Upgrade
                .WeaponActionRequirements);
        }
    }

    public class AppliedUpgrade
    {
        public Upgrade Upgrade { get; }

        public AppliedUpgrade(Upgrade upgrade)
        {
            Upgrade = upgrade;
        }
    }
}