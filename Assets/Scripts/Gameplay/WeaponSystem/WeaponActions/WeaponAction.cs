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

        public void TryExecute(WeaponActionExecutionData data, DiContainer container)
        {
            if (CanExecute(data.Owner)) Execute(data, container);
        }

        protected abstract void Execute(WeaponActionExecutionData data, DiContainer container);
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

        public void TryExecute(WeaponActionExecutionData data, DiContainer container, T arg)
        {
            if (CanExecute(data.Owner)) Execute(data, container, arg);
        }

        protected abstract void Execute(WeaponActionExecutionData data, DiContainer container, T character);
    }

    [Serializable]
    public abstract class WeaponActionOnCharacter : WeaponAction<Character>
    {
    }
}