using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    [CreateAssetMenu(fileName = "Character Data", menuName = "Character Data", order = 1)]
    public class CharacterData : ScriptableObject
    {
        [field: SerializeField] public string CharacterName { get; private set; }
        [field: SerializeField] public List<WeaponData> StartingWeapons { get; private set; }
        [field: SerializeField] public Character CharacterPrefab { get; private set; }
        [field: SerializeField] public CharacterStatOverrides StatOverrides { get; private set; }
    }
}