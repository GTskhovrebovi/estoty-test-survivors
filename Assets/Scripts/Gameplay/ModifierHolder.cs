using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Character))]
    public class ModifierHolder : MonoBehaviour
    {
        private readonly List<AppliedModifier> _appliedModifiers = new();
        private Character _character;
        private List<AppliedModifier> _modifiersToUpdate = new();
        public event Action<AppliedModifier> OnModifierApply;
        public event Action<AppliedModifier> OnModifierRemove;

        public List<AppliedModifier> AppliedModifiers => _appliedModifiers;

        public bool IsStunned { get; private set; }
        public bool IsInvisible { get; private set; }
        public bool CanSeeInvisible { get; private set; }

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

        public bool HasModifier(ModifierData modifierData)
        {
            foreach (var appliedModifier in AppliedModifiers)
            {
                if (appliedModifier.ModifierData == modifierData)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateEffects()
        {
            IsInvisible = false;
            IsStunned = false;
            CanSeeInvisible = false;
            
            foreach (var appliedModifier in _appliedModifiers)
            {
                foreach (var modifierEffect in appliedModifier.ModifierData.ModifierEffects)
                {
                    if (modifierEffect == ModifierEffect.Stun) IsStunned = true;
                    if (modifierEffect == ModifierEffect.Invisibility) IsInvisible = true;
                    if (modifierEffect == ModifierEffect.SeeInvisibility) CanSeeInvisible = true;
                }
            }
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
        
        public AppliedModifier ApplyModifier(ModifierData modifierData, Character caster, CharacterStats characterStats, Team team, Weapon weapon, int numberOfStacks)
        {
            AppliedModifier appliedModifier = null;
            if (modifierData.Stackable)
            {
                for (int i = 0; i < numberOfStacks; i++)
                {
                    appliedModifier = ApplyModifier(modifierData, caster, characterStats, team, weapon);
                }
            }
            else
            {
                appliedModifier = ApplyModifier(modifierData, caster, characterStats, team, weapon);
            }

            return appliedModifier;
        }
    
        public AppliedModifier ApplyModifier(ModifierData modifierData, Character caster, CharacterStats characterStats, Team team, Weapon weapon)
        {
            if (!_character.Alive) return null;

            var existingModifier = _appliedModifiers.FirstOrDefault(i => i.Caster == caster && i.ModifierData == modifierData);
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
                // Debug.Log($"{caster.gameObject.name} applied {modifier.name} on {gameObject.name}".Colorize(Color.cyan));

                var modifierDuration = modifierData.HasDuration
                    ? modifierData.Duration.Value(characterStats)
                    : Mathf.Infinity;
                var appliedModifier = new AppliedModifier(caster, characterStats, team, weapon, _character, modifierData, Time.time, modifierDuration)
                {
                    EndAction = RemoveAppliedModifier
                };

                _appliedModifiers.Add(appliedModifier);
                OnModifierApply?.Invoke(appliedModifier);
                appliedModifier.StartEffect();
                
                UpdateEffects();
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
            // Debug.Log($"{appliedModifier.Modifier.name} Modifier Removed from {gameObject.name}".Colorize(Color.white));
            _appliedModifiers.Remove(appliedModifier);
            OnModifierRemove?.Invoke(appliedModifier);
            UpdateEffects();
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