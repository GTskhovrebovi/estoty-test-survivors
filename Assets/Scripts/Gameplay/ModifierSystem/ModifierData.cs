using System;
using System.Collections.Generic;
using Gameplay.WeaponSystem.WeaponActions;
using UnityEngine;

namespace Gameplay.ModifierSystem
{
    [CreateAssetMenu(fileName = "New Modifier", menuName = "Gameplay/Modifier", order = 1)]
    public class ModifierData : ScriptableObject
    {
        [field: SerializeField] public bool Stackable { get; private set; }
        [field: SerializeField] public bool RemoveCurrent { get; private set; }
        [field: SerializeField] public bool ResetCurrentDuration { get; private set; }
        [field: SerializeField] public bool LoseStacksIndividually { get; private set; }

        [SerializeField] private bool hasDuration = true;
        [SerializeField] private Variable duration;
        [SerializeField] private List<StatModifier> statModifiers = new();
        [Space] [SerializeField] private bool hasTicker;
        [SerializeField] private Variable tickInterval;
        [SerializeField] private List<WeaponActionOnCharacter> onTickActionsOnCharacter = new();
        [field: SerializeField] public ColorChangeEffectData ColorOverrideData { get; private set; }

        public bool HasDuration => hasDuration;
        public Variable Duration => duration;
        public bool HasTicker => hasTicker;
        public Variable TickInterval => tickInterval;
        public List<WeaponActionOnCharacter> OnTickActionsOnCharacter => onTickActionsOnCharacter;
        public List<StatModifier> StatModifiers => statModifiers;
    }

    public class Ticker
    {
        private readonly float _tickInterval;
        private float _nextTickTime;

        private Action _tickCallback;

        public Ticker(float tickInterval, Action tickCallback)
        {
            _tickInterval = tickInterval;
            _tickCallback = tickCallback;
        }

        public void Start()
        {
            _nextTickTime = Time.time;
        }

        public void Tick()
        {
            if (Time.time >= _nextTickTime)
            {
                _tickCallback?.Invoke();
                _nextTickTime = Time.time + _tickInterval;
            }
        }
    }
}