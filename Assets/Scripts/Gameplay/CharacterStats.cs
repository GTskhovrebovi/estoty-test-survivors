using System;
using System.Collections.Generic;
using Gameplay.ModifierSystem;

namespace Gameplay
{
    [Serializable]
    public class CharacterStats
    {
        public Dictionary<StatType, Stat> Stats { get; private set; } = new();

        public void Initialize(List<CharacterStatOverride> baseStats)
        {
            Stats = new Dictionary<StatType, Stat>();
            AddStats(baseStats);
        }

        public Stat GetStat(StatType statType)
        {
            if (statType == null) return null;
            if (!Stats.TryGetValue(statType, out var stat))
            {
                stat = new Stat(statType.BaseValue);
                Stats.Add(statType, stat);
            }

            return stat;
        }

        public string GetStatValue(StatType statType)
        {
            return GetStat(statType).Value.ToString();
        }

        public void AddStat(StatType statType)
        {
            if (!Stats.ContainsKey(statType)) Stats.Add(statType, new Stat(statType.BaseValue));
        }

        public void AddStat(CharacterStatOverride statOverride)
        {
            if (Stats.ContainsKey(statOverride.StatType)) return;
            Stats.Add(statOverride.StatType, new Stat(statOverride.Value));
        }

        public void AddStats(List<StatType> stats)
        {
            foreach (var statType in stats)
            {
                AddStat(statType);
            }
        }

        private void AddStats(List<CharacterStatOverride> characterStatOverrides)
        {
            foreach (var statOverride in characterStatOverrides)
            {
                AddStat(statOverride);
            }
        }

        public AppliedStatModifier ApplyModifier(StatModifier statModifier, object source)
        {
            var appliedStatModifier = GetStat(statModifier.StatType).ApplyModifier(statModifier, source);
            return appliedStatModifier;
        }

        public List<AppliedStatModifier> ApplyModifiers(IEnumerable<StatModifier> statModifiers, object source)
        {
            var appliedModifiers = new List<AppliedStatModifier>();
            foreach (var statModifier in statModifiers)
            {
                appliedModifiers.Add(ApplyModifier(statModifier, source));
            }

            return appliedModifiers;
        }


        public void RemoveAllModifiersFromSource(object source)
        {
            //Debug.Log($"Removing modifiers of {source} ".Colorize(Color.red));
            foreach (var stat in Stats)
            {
                stat.Value.RemoveModifiersFromSource(source);
            }
        }

        public void OverrideStatBaseValue(StatType statType, float value)
        {
            var stat = GetStat(statType);
            stat.SetBaseValue(value);
        }
    }
}