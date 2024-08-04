using System;
using System.Collections.Generic;
using Gameplay.WeaponSystem;
using UnityEngine;

namespace Gameplay.ModifierSystem
{
    [Serializable]
    public class AppliedModifier
    {
        public Character Source { get; }
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

        public List<AppliedStatModifier> AppliedStatModifiers { get; private set; } = new();

        public AppliedModifier(Character source, CharacterStats characterStats, Team team, Weapon weapon,
            Character target, ModifierData modifierDataData, float applicationTime, float duration)
        {
            Source = source;
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
                onTickAction.TryExecute(new WeaponActionExecutionData(Source, CharacterStats, Team, Weapon),
                    Weapon.Container, Target);
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
            Target.CharacterStats.RemoveAllModifiersFromSource(this);
        }

        public void ResetDuration()
        {
            StartTime = Time.time;
        }
    }
}