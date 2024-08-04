using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider experienceSlider;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text killsText;
        [SerializeField] private TMP_Text ammoText;
        [SerializeField] private UpgradeInfoPopup upgradeInfoPopup;
        [SerializeField] private RewardsManager rewardsManager;

        private Character _player;

        private string _levelTextFormat;
        private string _killsTextFormat;
        private string _ammoTextFormat;

        private void Awake()
        {
            _levelTextFormat = levelText.text;
            _killsTextFormat = killsText.text;
            _ammoTextFormat = ammoText.text;
        }

        public void Initialize(Character player)
        {
            _player = player;
            _player.Health.OnHealthChanged += HandleHealthChange;
            _player.LevelingSystem.OnExperienceChange += HandleExperienceChange;
            _player.OnCharacterKill += HandleCharacterKill;
            _player.OnAmmoChange += HandleAmmoChange;
            _player.OnMaxAmmoChange += HandleMaxAmmoChange;
            rewardsManager.OnRewardGrant += HandleRewardGrant;

            UpdateHealth();
            UpdateLevelState();
            UpdateKills();
            UpdateAmmo();
        }

        private void HandleRewardGrant(IRewardAction rewardAction)
        {
            switch (rewardAction)
            {
                case AddUpgrade addUpgrade:
                    upgradeInfoPopup.Show(addUpgrade);
                    break;
                case AddWeapon addWeapon:
                    upgradeInfoPopup.Show(addWeapon);
                    break;
            }
        }

        private void HandleCharacterKill(Character _)
        {
            UpdateKills();
        }

        private void HandleHealthChange()
        {
            UpdateHealth();
        }

        private void HandleExperienceChange()
        {
            UpdateLevelState();
        }

        private void HandleAmmoChange(int amount)
        {
            UpdateAmmo();
        }

        private void HandleMaxAmmoChange(int amount)
        {
            UpdateAmmo();
        }

        private void UpdateKills()
        {
            killsText.text = string.Format((_killsTextFormat), _player.NumberOfKills);
        }

        private void UpdateAmmo()
        {
            ammoText.text = string.Format((_ammoTextFormat), _player.CurrentAmmoAmount, _player.MaxAmmoAmount);
        }

        private void UpdateHealth()
        {
            healthSlider.value = _player.Health.HealthFraction;
        }

        private void UpdateLevelState()
        {
            var levelState = _player.LevelingSystem.LevelState;
            experienceSlider.value = levelState.Progress;
            levelText.text = string.Format(_levelTextFormat, levelState.Level);
        }

        private void OnDestroy()
        {
            if (_player != null)
            {
                _player.Health.OnHealthChanged -= HandleHealthChange;
                _player.LevelingSystem.OnExperienceChange -= HandleExperienceChange;
                _player.OnCharacterKill -= HandleCharacterKill;
                _player.OnAmmoChange -= HandleAmmoChange;
                rewardsManager.OnRewardGrant -= HandleRewardGrant;
            }
        }
    }
}