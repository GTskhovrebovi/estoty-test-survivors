using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public abstract class WeaponAction : ScriptableObject
    {
        public abstract void Execute(WeaponActionExecutionData data);
    }
    
    [Serializable]
    public abstract class WeaponAction<T> : ScriptableObject
    {
        public abstract void Execute(WeaponActionExecutionData data, T character);
    }
    
    [Serializable]
    public abstract class WeaponActionOnCharacter : WeaponAction<Character>
    {
    }
}