using System;

namespace Gameplay.ModifierSystem
{
    [Serializable]
    public class AppliedStatModifier
    {
        public StatModifier statModifier;
        public object source;
        private float originalValue;

        public StatModifier StatModifier => statModifier;
        public object Source => source;
        public event Action OnValueChange;
        public float OriginalValue => originalValue;

        public AppliedStatModifier(StatModifier statModifier, object source)
        {
            this.statModifier = statModifier;
            this.source = source;
            originalValue = statModifier.Value;
        }

        public void UpdateValue(float newValue)
        {
            statModifier.UpdateValue(newValue);
            OnValueChange?.Invoke();
        }
    }
}