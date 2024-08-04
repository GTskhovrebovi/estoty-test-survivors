using System;
using UnityEngine;

namespace Gameplay.ModifierSystem
{
    [Serializable]
    public struct StatModifier
    {
        [SerializeField] private StatType statType;
        [SerializeField] private StatModifierType modifierType;
        [SerializeField] private float value;
        [SerializeField] private int priority;

        public StatType StatType => statType;
        public StatModifierType ModifierType => modifierType;
        public float Value => value;
        public int Priority => priority;

        public StatModifier(StatType statType, StatModifierType modifierType, float value, int priority = 0)
        {
            this.statType = statType;
            this.modifierType = modifierType;
            this.value = value;
            this.priority = priority;
        }

        public void UpdateValue(float newValue)
        {
            value = newValue;
        }

        public StatModifier Multiply(int amount)
        {
            return new StatModifier(statType, modifierType, value * amount);
        }
    }

    public enum StatModifierType
    {
        Flat,
        PercentAdd,
        PercentMult,
        Set,
        Override
    }

    public struct DynamicStatModifier
    {
        [SerializeField] private StatType statType;
        [SerializeField] private StatModifierType modifierType;
        [SerializeField] private Variable value;
        [SerializeField] private int priority;

        public DynamicStatModifier(StatType statType, StatModifierType modifierType, Variable value, int priority = 0)
        {
            this.statType = statType;
            this.modifierType = modifierType;
            this.value = value;
            this.priority = priority;
        }

        public StatModifier GetStatModifier(CharacterStats stats)
        {
            return new StatModifier(
                statType,
                modifierType,
                value.Value(stats),
                priority
            );
        }
    }
}