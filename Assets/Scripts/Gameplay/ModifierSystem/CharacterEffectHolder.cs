using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.ModifierSystem
{
    public class CharacterEffectHolder : MonoBehaviour
    {
        private Character _character;
        private SpriteRenderer _spriteRenderer;
        private Color _initialColor;
        private Dictionary<AppliedModifier, ColorChangeEffectData> _appliedColorOverrides = new();
        private ModifierHolder _modifierHolder;
        
        private void Awake()
        {
            _character = GetComponent<Character>();
            _modifierHolder = GetComponent<ModifierHolder>();
            _spriteRenderer = _character.GraphicsSpriteRenderer;
            _initialColor = _spriteRenderer.color;
        }

        private void OnEnable()
        {
            _modifierHolder.OnModifierApply += HandleModifierApply;
            _modifierHolder.OnModifierRemove += HandleModifierRemove;
        }

        private void OnDisable()
        {
            _modifierHolder.OnModifierApply -= HandleModifierApply;
            _modifierHolder.OnModifierRemove -= HandleModifierRemove;
        }

        private void HandleModifierApply(AppliedModifier appliedModifier)
        {
            ApplyEffect(appliedModifier);
        }

        private void HandleModifierRemove(AppliedModifier appliedModifier)
        {
            RemoveEffectsOfAppliedModifier(appliedModifier);
        }

        private void ApplyEffect(AppliedModifier appliedModifier)
        {
            if (appliedModifier.ModifierData.ColorOverrideData == null) return;
            if (_appliedColorOverrides.ContainsKey(appliedModifier)) return;

            _appliedColorOverrides.Add(appliedModifier, appliedModifier.ModifierData.ColorOverrideData);
            UpdateColor();
        }

        private void RemoveEffectsOfAppliedModifier(AppliedModifier appliedModifier)
        {
            if (_appliedColorOverrides.ContainsKey(appliedModifier))
            {
                _appliedColorOverrides.Remove(appliedModifier);
                UpdateColor();
            }
        }

        private void UpdateColor()
        {
            if (_spriteRenderer == null) return;
            if (!_appliedColorOverrides.Any())
            {
                _spriteRenderer.color = _initialColor;
            }
            else
            {
                var colorSum = new Color(0, 0, 0, 0);

                foreach (var (_, colorChangeEffectData) in _appliedColorOverrides)
                {
                    colorSum += colorChangeEffectData.NewColor;
                }

                var colorCount = _appliedColorOverrides.Count;
                var finalColor = new Color(
                    colorSum.r / colorCount,
                    colorSum.g / colorCount,
                    colorSum.b / colorCount,
                    colorSum.a / colorCount
                );

                _spriteRenderer.color = finalColor;
            }
        }
    }
}