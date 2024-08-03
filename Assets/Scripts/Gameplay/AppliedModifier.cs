using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class AppliedModifier
    {
        public Character Caster { get; }
        public CharacterStats CharacterStats { get; }
        public Team Team { get; }
        public Weapon Weapon { get; }
        public float ApplicationTime { get; }
        public float StartTime { get; private set; }
        public float Duration { get; }
        
        public ModifierData ModifierData { get; }
        public int NumberOfStacks { get; private set; }

        public Character Target { get; }
        private Ticker _ticker;

        public Action<AppliedModifier> EndAction;
        public Action OnExpire;
        
        public float ExpirationTime => StartTime + Duration;
        public float NormalizedProgress => (Time.time - StartTime)/Duration;

        public List<AppliedStatModifier> AppliedStatModifiers { get; private set; } = new();
        
        public AppliedModifier(Character caster, CharacterStats characterStats, Team team, Weapon weapon, Character target, ModifierData modifierDataData, float applicationTime, float duration)
        {
            Caster = caster;
            CharacterStats = characterStats; 
            Team = team;
            Weapon = weapon;
            Target = target;
            ModifierData = modifierDataData;
            ApplicationTime = applicationTime;
            Duration = duration;
            NumberOfStacks = 1;
            StartTime = ApplicationTime;
        }

        public void AddStack()
        {
            NumberOfStacks++;
        }

        private void RemoveStack()
        {
            if (ModifierData.LoseStacksIndividually)
            {
                if (NumberOfStacks <= 1)
                    EndModifier();
                else
                { 
                    NumberOfStacks--;
                    ResetDuration();
                }
            }
            else
            {
                EndModifier();
            }
        }
        
        private void HandleOnTick()
        {
            foreach (var onTickAction in ModifierData.OnTickActionsOnCharacter)
            {
                onTickAction.Execute(new WeaponActionExecutionData(Caster, CharacterStats, Team, Weapon), Target);
            }
        }
    
        
        public void Update()
        {
            _ticker?.Tick();

            if (ModifierData.HasDuration && Time.time > ExpirationTime)
            {
                RemoveStack();
            }
        }
        
        public void StartEffect()
        {
            var statModifiers = new List<StatModifier>(ModifierData.StatModifiers);
            
            AppliedStatModifiers = Target.CharacterStats.ApplyModifiers(statModifiers, this);
            
            if (ModifierData.HasTicker)
            {
                _ticker = new Ticker(ModifierData.TickInterval.Value(CharacterStats), HandleOnTick);
                _ticker.Start();
            }
        }

        public void EndModifier(bool expire = true)
        {
            OnExpire?.Invoke();
            if (expire) EndAction?.Invoke(this);
            // Target.CharacterStats.RemoveModifiers(Modifier.StatModifiers);
            Target.CharacterStats.RemoveAllModifiersFromSource(this);
        }
        
        public void ResetDuration()
        {
            StartTime = Time.time;
        }
    }
}