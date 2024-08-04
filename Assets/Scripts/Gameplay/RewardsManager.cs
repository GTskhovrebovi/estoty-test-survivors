using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class RewardsManager : MonoBehaviour
    {
        private WeaponLibrary _weaponLibrary;
        private UpgradeLibrary _upgradeLibrary;
        private Character _character;
        private LevelingSystem _levelingSystem;
        private int _currentNumberOfLevels;
        
        public Action<IRewardAction> OnRewardGrant;
        
        [Inject]
        public void Construct(WeaponLibrary weaponLibrary, UpgradeLibrary upgradeLibrary)
        {
            _weaponLibrary = weaponLibrary;
            _upgradeLibrary = upgradeLibrary;
        }
        
        public void Initialize(Character player)
        {
            _character = player;
            _levelingSystem = player.GetComponent<LevelingSystem>();
            _levelingSystem.OnLevelUp += HandleLevelUp;
        }

        private void OnDestroy()
        {
            if (_levelingSystem != null)
                _levelingSystem.OnLevelUp -= HandleLevelUp;
        }

        private void HandleLevelUp(int numberOfLevels)
        {
            _currentNumberOfLevels = numberOfLevels;

            for (int i = 0; i < _currentNumberOfLevels; i++)
            {
                GrantRandomReward();
            }
        }

        [ContextMenu("GrantRandomReward")]
        private void GrantRandomReward()
        {
            var reward = GenerateRandomReward();
            reward.Execute(_character);
            OnRewardGrant?.Invoke(reward);
        }
        
        private IRewardAction GenerateRandomReward()
        {
            IRewardAction generatedRewards;
            
            var weaponSlotsFull = _character.WeaponUser.WeaponSlotsFull;
            var newWeapon = Random.value < 0.4f && !weaponSlotsFull;
            
            if (newWeapon)
            {
                var weaponData = _weaponLibrary.Weapons[Random.Range(0, _weaponLibrary.Weapons.Count)];
                generatedRewards = new AddWeapon(weaponData);
            }
            else
            {
                var upgrade = _upgradeLibrary.Upgrades[Random.Range(0, _upgradeLibrary.Upgrades.Count)];
                generatedRewards = new AddUpgrade(upgrade);
            }
            
            return generatedRewards;
        }
    }
}