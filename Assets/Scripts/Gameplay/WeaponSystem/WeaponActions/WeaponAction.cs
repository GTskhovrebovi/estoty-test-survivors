using System;
using UnityEngine;
using Zenject;

namespace Gameplay.WeaponSystem.WeaponActions
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

        public void TryExecute(WeaponActionExecutionData data, GameplayContext context)
        {
            if (CanExecute(data.Owner)) Execute(data, context);
        }

        protected abstract void Execute(WeaponActionExecutionData data, GameplayContext context);
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

        public void TryExecute(WeaponActionExecutionData data, GameplayContext context, T arg)
        {
            if (CanExecute(data.Owner)) Execute(data, context, arg);
        }

        protected abstract void Execute(WeaponActionExecutionData data, GameplayContext context, T character);
    }

    [Serializable]
    public abstract class WeaponActionOnCharacter : WeaponAction<Character>
    {
    }
}