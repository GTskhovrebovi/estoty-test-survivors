using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "Character Stats", menuName = "Character Stats", order = 1)]
    public class CharacterStatOverrides : ScriptableObject
    {
        [field: SerializeField] public List<CharacterStatOverride> BaseStats { get; private set; }
    }
    
    [Serializable]
    public class CharacterStatOverride
    {
        [field: SerializeField] public StatType StatType { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }
}