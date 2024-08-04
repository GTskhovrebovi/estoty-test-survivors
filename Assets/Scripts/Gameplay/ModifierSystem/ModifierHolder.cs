using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.WeaponSystem;
using UnityEngine;

namespace Gameplay.ModifierSystem
{
    [RequireComponent(typeof(Character))]
    public class ModifierHolder : MonoBehaviour
    {
        private readonly List<AppliedModifier> _appliedModifiers = new();
        private Character _character;
        private List<AppliedModifier> _modifiersToUpdate = new();

        public event Action<AppliedModifier> OnModifierApply;
        public event Action<AppliedModifier> OnModifierRemove;
        
        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void OnEnable()
        {
            _character.OnDeath += HandleCharacterDeath;
            _character.OnEndExistence += HandleCharacterEndExistence;
        }
        
        
        private void OnDisable()
        {
            _character.OnDeath -= HandleCharacterDeath;
            _character.OnEndExistence -= HandleCharacterEndExistence;
        }

        private void HandleCharacterDeath()
        {
            EndAllModifiers();
        }
        
        private void HandleCharacterEndExistence(Character character)
        {
            EndAllModifiers();
        }

        private void EndAllModifiers()
        {
            var cachedAppliedModifiers = _appliedModifiers.ToList();
            foreach (var appliedModifier in cachedAppliedModifiers)
            {
                EndModifier(appliedModifier);
            }
        }

        private void Update()
        {
            UpdateModifiers();
        }

        private void UpdateModifiers()
        {
            _modifiersToUpdate.Clear();
            _modifiersToUpdate.AddRange(_appliedModifiers);

            for (var i = 0; i < _modifiersToUpdate.Count; i++)
            {
                var modifier = _modifiersToUpdate[i];
                if (_appliedModifiers.Contains(modifier))
                    modifier.Update();
            }
            
            _modifiersToUpdate.Clear();
        }
    
        public AppliedModifier ApplyModifier(ModifierData modifierData, Character source, CharacterStats characterStats, Team team, Weapon weapon)
        {
            if (!_character.Alive) return null;

            var existingModifier = _appliedModifiers.FirstOrDefault(i => i.Source == source && i.ModifierData == modifierData);
            if (modifierData.Stackable)
            {
                if (existingModifier != null)
                {
                    existingModifier.AddStack();

                    if (modifierData.ResetCurrentDuration)
                    {
                        existingModifier.ResetDuration();
                    }

                    return existingModifier;
                }
                else
                {
                    var appliedModifier = Apply();
                    return appliedModifier;
                }
            }
            else
            {
                if (modifierData.RemoveCurrent && existingModifier != null)
                {
                    EndModifier(existingModifier);
                }
                
                var appliedModifier = Apply();
                return appliedModifier;
            }
            
            AppliedModifier Apply()
            {
                var modifierDuration = modifierData.HasDuration
                    ? modifierData.Duration.Value(characterStats)
                    : Mathf.Infinity;
                var appliedModifier = new AppliedModifier(source, characterStats, team, weapon, _character, modifierData, Time.time, modifierDuration)
                {
                    EndAction = RemoveAppliedModifier
                };

                _appliedModifiers.Add(appliedModifier);
                OnModifierApply?.Invoke(appliedModifier);
                appliedModifier.StartEffect();
                return appliedModifier;
            }
        }

        public void RemoveModifier(ModifierData modifierData, bool expire = true)
        {
            var appliedModifier = _appliedModifiers.FirstOrDefault(i => i.ModifierData == modifierData);
            if (appliedModifier != null)
            {
                EndModifier(appliedModifier);
            }
        }

        private void EndModifier(AppliedModifier appliedModifier)
        {
            appliedModifier.EndModifier();
        }
        
        private void RemoveAppliedModifier(AppliedModifier appliedModifier)
        {
            _appliedModifiers.Remove(appliedModifier);
            OnModifierRemove?.Invoke(appliedModifier);
        }

        public void RemoveAllModifiers()
        {
            for (var i = 0; i < _appliedModifiers.Count; i++)
            {
                var appliedModifier = _appliedModifiers[i];
                RemoveModifier(appliedModifier.ModifierData, false);
            }
        }
    }
}