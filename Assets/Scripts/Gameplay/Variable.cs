using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class Variable
    {
        [SerializeField] private VariableType variableType;
        [SerializeField] private float constantValue = 1;
        [SerializeField] private StatType statType;

        public Variable(VariableType variableType, float constantValue, StatType statType)
        {
            this.variableType = variableType;
            this.constantValue = constantValue;
            this.statType = statType;
        }

        public float Value(CharacterStats characterStats) =>
            variableType switch
            {
                VariableType.Constant => constantValue,
                VariableType.Variable => characterStats.GetStat(statType).Value,
                _ => 0
            };
    }

    public enum VariableType
    {
        Constant,
        Variable,
        Expression
    }
}