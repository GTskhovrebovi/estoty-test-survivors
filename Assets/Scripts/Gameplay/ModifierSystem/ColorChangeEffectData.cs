using System;
using UnityEngine;

namespace Gameplay.ModifierSystem
{
    [Serializable]
    [CreateAssetMenu(fileName = "MaterialChangeEffectData", menuName = "Gameplay/MaterialChangeEffectData", order = 1)]
    public class ColorChangeEffectData : ScriptableObject
    {
        [field: SerializeField] public Color NewColor { get; private set; }
    }
}