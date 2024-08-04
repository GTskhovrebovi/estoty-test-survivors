using System;
using UnityEngine;

namespace Gameplay.WeaponSystem
{
    public class WeaponObject : MonoBehaviour
    {
        [field: SerializeField] public bool HasLifeTime { get; set; }
        [field: SerializeField] public float LifeTime { get; set; }
        public Action<WeaponObject> EndAction { get; set; }
        public Character Owner { get; protected set; }
        public CharacterStats CharacterStats { get; protected set; }
        public Team Team { get; protected set; }
        public Weapon Weapon { get; protected set; }
        
        protected bool IsActive { get; set; }
        protected float StartTime;
        protected float InitialLifeTime;
        
        protected virtual void Awake()
        {
            InitialLifeTime = LifeTime;
        }

        public void Initialize(Character owner, CharacterStats characterStats, Team team, Weapon weapon)
        {
            Owner = owner;
            CharacterStats = characterStats;
            Team = team;
            Weapon = weapon;
            StartBehavior();
        }

        public virtual void StartBehavior()
        {
            IsActive = true;
            StartTime = Time.time;
        }

        protected virtual void Update()
        {
            if (IsActive) CheckLifeTime();
        }

        private void CheckLifeTime()
        {
            if (!HasLifeTime) return;
            if (Time.time > StartTime + LifeTime)
                End();
        }

        protected virtual void End()
        {
            if (!IsActive) return;
            IsActive = false;
            EndAction?.Invoke(this);
        }
    }
}