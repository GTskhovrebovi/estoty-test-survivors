using System;
using UnityEngine;

namespace Gameplay
{
    public class LevelingSystem : MonoBehaviour
    {
        private LevelState _levelState;
        private int _totalExperience;
        
        public LevelState LevelState => _levelState;
        
        public event Action OnExperienceChange;
        public event Action<int> OnLevelUp;
        
        protected  void Awake()
        {
            SetExperience(0);
        }
        
        public void GrantExperience(int amount)
        {
            _totalExperience += amount;
            UpdateLevelState();
        }
        
        public void GrantLevel()
        {
            GrantExperience(LevelState.TotalExperienceForNextLevel - LevelState.Experience);
            UpdateLevelState();
        }

        private void SetExperience(int amount)
        {
            _totalExperience = amount;
            UpdateLevelState();
        }
        
        private void UpdateLevelState()
        {
            var oldLevelState = _levelState;
            _levelState = CalculateLevelState(_totalExperience);
            OnExperienceChange?.Invoke();
            if (oldLevelState.Level != _levelState.Level)
            {
                OnLevelUp?.Invoke(_levelState.Level - oldLevelState.Level);
            }
        }
        
        private static LevelState CalculateLevelState(int totalExperience)
        {
            var experienceLeft = totalExperience;
            var currentLevel = 1;
            
            while (true)
            {
                var experienceForNextLevel = ExperienceForNextLevel(currentLevel);
                if (experienceLeft >= experienceForNextLevel)
                {
                    experienceLeft -= experienceForNextLevel;
                    currentLevel++;
                }
                else
                {
                    return new LevelState()
                    {
                        Experience = experienceLeft,
                        Level = currentLevel,
                    };
                }
            }
        }

        public static int ExperienceForNextLevel(int level)
        {
            return 100 + 40 * level + 10 * level * level;
        }
    }
    
    [Serializable]
    public struct LevelState
    {
        [field: SerializeField] public int Experience { get; set; }
        [field: SerializeField] public int Level { get; set; }
        public int TotalExperienceForNextLevel => LevelingSystem.ExperienceForNextLevel(Level);
        public float Progress => (float)Experience / TotalExperienceForNextLevel;
    }
}