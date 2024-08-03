using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private bool aimAtTarget;
        [SerializeField] private float aimRange = 10f;
        [SerializeField] private StatType fireRate;
        [SerializeField] private Animator animator;
        [field: SerializeField] public Transform SpawnPoint { get; private set; }
        public WeaponData WeaponData { get; private set; }
        public Character Owner { get; private set; }
        public WeaponUser WeaponUser { get;  private set; }
        public Character Target { get;  private set; }
        private Collider2D[] _results = new Collider2D[20];
        private float _remainingCooldown;
        private readonly Action<Weapon> _castCallback;
        private readonly List<AppliedModifier> _bindedModifiers = new();
        private static readonly int UseHash = Animator.StringToHash("Use");

        public void BindModifier(AppliedModifier appliedModifier)
        {
            _bindedModifiers.Add(appliedModifier);
        }
    
        public void Initialize(Character owner, WeaponUser weaponUser, WeaponData weaponData)
        {
            Owner = owner;
            WeaponUser = weaponUser;
            WeaponData = weaponData;
            _remainingCooldown = 0;
        }

        public void Update()
        {
            if (aimAtTarget)
            {
                FindTarget();
                RotateTowardsTarget();
            }
            
            _remainingCooldown -= Time.deltaTime * Owner.CharacterStats.GetStat(fireRate).Value;
            _remainingCooldown = Mathf.Max(0, _remainingCooldown);

            TryUse();
        }

        public void TryUse()
        {
            if (_remainingCooldown <= 0)
            {
                Use();
            }
        }
    
        protected virtual void Use()
        {
            _remainingCooldown = WeaponData.CooldownVariable.Value(Owner.CharacterStats);
            animator.SetTrigger(UseHash);
        }
        
        public void FindTarget()
        {
            var closestDistance = Mathf.Infinity;
            Target = null;

            var size = Physics2D.OverlapCircleNonAlloc(transform.position, aimRange, _results, Layers.CharacterLayerMask);

            for (var i = 0; i < size; i++)
            {
                var character = _results[i].GetComponent<Character>();
                if (character == null) continue;
                if (Owner.Team.IsAlly(character.Team)) continue;

                var distance = Vector2.Distance(transform.position, character.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    Target = character;
                }
            }
        }
        
        private void RotateTowardsTarget()
        {
            if (Target == null) return;
            transform.right = Target.transform.position - transform.position;
        }
    }
}