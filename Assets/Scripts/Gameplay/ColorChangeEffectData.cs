using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    [CreateAssetMenu(fileName = "MaterialChangeEffectData", menuName = "MaterialChangeEffectData", order = 1)]
    public class ColorChangeEffectData : ScriptableObject
    {
        [field: SerializeField] public Color NewColor { get; private set; }
    }
}