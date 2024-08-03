using System;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay
{
    [Serializable]
    public class Stat
    {
        private float _baseValue;
        private readonly List<AppliedStatModifier> _appliedStatModifiers = new();
    
        public float Value { get; private set; }

        public event Action OnStatChanged;
    
        public Stat(float baseValue)
        {
            _baseValue = baseValue;
            CalculateFinalValue();
        }

        public void SetBaseValue(float baseValue)
        {
            _baseValue = baseValue;
            CalculateFinalValue();
            OnStatChanged?.Invoke();
        }

        private void CalculateFinalValue()
        {
            var baseValue = _baseValue;

            var setModifiers = _appliedStatModifiers.Where(m => m.StatModifier.ModifierType == StatModifierType.Set).ToList();
            if (setModifiers.Any())
            {
                Value = setModifiers.OrderByDescending(i => i.StatModifier.Priority).First().StatModifier.Value;
            }
        
            var overrideModifiers = _appliedStatModifiers.Where(m => m.StatModifier.ModifierType == StatModifierType.Override).ToList();
            if (overrideModifiers.Any())
            {
                baseValue = overrideModifiers.OrderByDescending(i => i.StatModifier.Priority).First().StatModifier.Value;
            }
        
            float totalFlat = 0;
            foreach (var modifier in _appliedStatModifiers.Where(m => m.StatModifier.ModifierType == StatModifierType.Flat))
            {
                totalFlat += modifier.StatModifier.Value;
            }
        
            float totalPercentAdd = 0;
            foreach (var modifier in _appliedStatModifiers.Where(m => m.StatModifier.ModifierType == StatModifierType.PercentAdd))
            {
                totalPercentAdd += modifier.StatModifier.Value / 100;
            }
        
            float totalPercentMult = 1;
            foreach (var modifier in _appliedStatModifiers.Where(m => m.StatModifier.ModifierType == StatModifierType.PercentMult))
            {
                totalPercentMult *= 1 + (modifier.StatModifier.Value / 100);
            }

            var finalValue = baseValue + totalFlat;
            finalValue += finalValue * totalPercentAdd;
            finalValue *= totalPercentMult;

            Value = (float)Math.Round(finalValue, 4);
        }

        public void RemoveModifiersFromSource(object source)
        {
            for (int i = _appliedStatModifiers.Count-1; i >= 0; i--)
            {
                var appliedStatModifier = _appliedStatModifiers[i];
                if (appliedStatModifier.Source != source) continue;
                _appliedStatModifiers.Remove(appliedStatModifier);
                appliedStatModifier.OnValueChange -= HandleModifierValueChange;
            }
        
            CalculateFinalValue();
            OnStatChanged?.Invoke();
        }

        public AppliedStatModifier ApplyModifier(StatModifier statModifier, object source)
        {
            var appliedStatModifier = new AppliedStatModifier(statModifier, source);
            _appliedStatModifiers.Add(appliedStatModifier);
            appliedStatModifier.OnValueChange += HandleModifierValueChange;
            CalculateFinalValue();
            OnStatChanged?.Invoke();
            return appliedStatModifier;
        }

        private void HandleModifierValueChange()
        {
            CalculateFinalValue();
            OnStatChanged?.Invoke();
        }
    }
}