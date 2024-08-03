using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public abstract class WeaponAction : ScriptableObject
    {
        [field: SerializeField] public WeaponActionRequirement Requirement { get; set; }
        
        protected bool CanExecute(Character character)
        {
            return (Requirement == null 
                    || (Requirement != null && character.WeaponActionRequirementHolder.HasRequirement(Requirement)));
        }
        
        public void TryExecute(WeaponActionExecutionData data)
        {
            if (CanExecute(data.Owner)) Execute(data);
        }
        
        protected abstract void Execute(WeaponActionExecutionData data);
    }
    
    [Serializable]
    public abstract class WeaponAction<T> : ScriptableObject
    {
        [field: SerializeField] public WeaponActionRequirement Requirement { get; set; }
        
        protected bool CanExecute(Character character)
        {
            return (Requirement == null 
                    || (Requirement != null && character.WeaponActionRequirementHolder.HasRequirement(Requirement)));
        }
        
        public void TryExecute(WeaponActionExecutionData data, T arg)
        {
            if (CanExecute(data.Owner)) Execute(data, arg);
        }
        protected abstract void Execute(WeaponActionExecutionData data, T character);
    }
    
    [Serializable]
    public abstract class WeaponActionOnCharacter : WeaponAction<Character>
    {
    }
}