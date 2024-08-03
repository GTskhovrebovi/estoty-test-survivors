using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider experienceSlider;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text killsText;

        private Character _player;

        private string _levelTextFormat;
        private string _killsTextFormat;
    
        private void Awake()
        {
            _levelTextFormat = levelText.text;
            _killsTextFormat = killsText.text;
        }

        public void Initialize(Character player)
        {
            _player = player;
            _player.Health.OnHealthChanged += HandleHealthChange;
            _player.LevelingSystem.OnExperienceChange += HandleExperienceChange;
            _player.OnCharacterKill += HandleCharacterKill;
        
            UpdateHealth();
            UpdateLevelState();
            UpdateKills();
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
    
        private void UpdateKills()
        {
            killsText.text = string.Format((_killsTextFormat), _player.NumberOfKills);
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
            }
        }
    }
}