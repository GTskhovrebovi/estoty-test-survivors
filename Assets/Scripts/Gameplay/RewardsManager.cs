using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class RewardsManager : MonoBehaviour
    {
        [SerializeField] private List<Upgrade> upgrades;
        [SerializeField] private List<WeaponData> weapons;
        
        private Character _character;
        private LevelingSystem _levelingSystem;
        private int _currentNumberOfLevels;
        
        public Action<IRewardAction> OnRewardGrant;
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
                var weaponData = weapons[Random.Range(0, weapons.Count)];
                generatedRewards = new AddWeapon(weaponData);
            }
            else
            {
                var upgrade = upgrades[Random.Range(0, upgrades.Count)];
                generatedRewards = new AddUpgrade(upgrade);
            }
            
            return generatedRewards;
        }
    }
}