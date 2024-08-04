using System;
using System.Collections.Generic;
using Gameplay.WeaponSystem;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    [CreateAssetMenu(fileName = "Character Data", menuName = "Gameplay/Character Data", order = 1)]
    public class CharacterData : ScriptableObject
    {
        [field: SerializeField] public string CharacterName { get; private set; }
        [field: SerializeField] public List<WeaponData> StartingWeapons { get; private set; }
        [field: SerializeField] public Character CharacterPrefab { get; private set; }
        [field: SerializeField] public List<CharacterStatOverride> BaseStats { get; private set; }
    }

    [Serializable]
    public class CharacterStatOverride
    {
        [field: SerializeField] public StatType StatType { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }
}