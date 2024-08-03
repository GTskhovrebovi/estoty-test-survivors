using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private Transform graphics;
        [SerializeField] private SpriteRenderer graphicsSpriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private StatType movementSpeedStat;
        [SerializeField] private StatType healthStat;
        [SerializeField] private StatType regenerationStat;
        [SerializeField] private StatType maxAmmoStat;
    
        public Health Health { get; private set; }
        public ModifierHolder ModifierHolder { get; private set; }
        public UpgradeHolder UpgradeHolder { get; private set; }
        public WeaponUser WeaponUser { get; private set; }
        public LevelingSystem LevelingSystem { get; private set; }
        public Rigidbody2D RigidBody { get; private set; }
        public WeaponActionRequirementHolder WeaponActionRequirementHolder { get; private set; }
        public Team Team { get; set; }
        public Vector2 MovementDirection { get; set; }
        public Vector2 FacingDirection { get; private set; }
        public bool Alive { get; private set; }
        public int CurrentAmmoAmount { get; private set; }
        public int MaxAmmoAmount { get; private set; }
        public int NumberOfKills { get; private set; }
        public bool HasAmmo => CurrentAmmoAmount > 0;
        public SpriteRenderer GraphicsSpriteRenderer => graphicsSpriteRenderer;
        
        private Stat _movementStat;
        private Stat _maxAmmoStat;
        
        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Dead = Animator.StringToHash("Dead");
    
        public event Action OnDeath;
        public event Action<Character> OnEndExistence;
        public event Action<Character> OnCharacterKill;
        public event Action<int> OnAmmoChange;
        public CharacterStats CharacterStats { get; private set; }
        public Vector3 Center => _collider2D.bounds.center;
        private CircleCollider2D _collider2D;
        protected void Awake()
        {
            _collider2D = GetComponent<CircleCollider2D>();
            CharacterStats = new CharacterStats();
            Health = GetComponent<Health>();
            ModifierHolder = GetComponent<ModifierHolder>();
            UpgradeHolder = GetComponent<UpgradeHolder>();
            WeaponUser = GetComponent<WeaponUser>();
            RigidBody = GetComponent<Rigidbody2D>();
            LevelingSystem = GetComponent<LevelingSystem>();
            WeaponActionRequirementHolder = GetComponent<WeaponActionRequirementHolder>();
            _movementStat = CharacterStats.GetStat(movementSpeedStat);
        }

        public void Initialize(CharacterStatOverrides statOverrides, Team team, List<WeaponData> startingWeapons)
        {
            CharacterStats = new CharacterStats();
            CharacterStats.Initialize(statOverrides);
            Health.Initialize(CharacterStats.GetStat(healthStat), CharacterStats.GetStat(regenerationStat), HandleHealthReachZero);
            _movementStat = CharacterStats.GetStat(movementSpeedStat);
            _maxAmmoStat = CharacterStats.GetStat(maxAmmoStat);
            if (WeaponUser != null) WeaponUser.Initialize(startingWeapons);
            Alive = true;
            _collider2D.enabled = true;
            Team = team;
            MaxAmmoAmount = Mathf.FloorToInt(_maxAmmoStat.Value);
            CurrentAmmoAmount = MaxAmmoAmount; 
            _maxAmmoStat.OnStatChanged += HandleMaxAmmoChange;
        }

        [ContextMenu("Gethit80")]
        public void GetHit80()
        {
            GetHit(new DamageEventArgs(this, this, 80));
        }
    
        public void GetHit(DamageEventArgs damageEventArgs)
        {
            if (Health == null) return;
            if (!Alive) return;

            Health.GetHit(damageEventArgs.Amount);
            // Debug.Log($"{name} taking damage: {damageEventArgs.Amount} Health: {Health.CurrentHealth}/{Health.TotalHealth}");
            animator.SetTrigger(Hit);
            
            if (damageEventArgs.Source != null)
                damageEventArgs.Source.HandleDamageDeal(damageEventArgs);
        }
    
        private void HandleDamageDeal(DamageEventArgs damageEventArgs)
        {
            if (!damageEventArgs.Target.Alive)
            {
                NumberOfKills++;
                OnCharacterKill?.Invoke(damageEventArgs.Target);
            }
        }
        
        private void HandleHealthReachZero()
        {
            Die();
        }
    
        [ContextMenu("Die")]
        private async void Die()
        {
            Alive = false;
            _collider2D.enabled = false;
            animator.SetBool(Dead, true);
            OnDeath?.Invoke();
            
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            OnEndExistence?.Invoke(this);
        }
    
        public void GrantExperience(int amount)
        {
            var totalAmount = Mathf.FloorToInt(amount);
            LevelingSystem.GrantExperience(totalAmount);
        }
        
        private void HandleMaxAmmoChange()
        {
            MaxAmmoAmount = Mathf.FloorToInt(_maxAmmoStat.Value);
            CurrentAmmoAmount = Mathf.Clamp(CurrentAmmoAmount, 0, MaxAmmoAmount);
        }
        
        public void GrantAmmo(int amount)
        {
            CurrentAmmoAmount += amount;
            CurrentAmmoAmount = Mathf.Clamp(CurrentAmmoAmount, 0, MaxAmmoAmount);
            OnAmmoChange?.Invoke(CurrentAmmoAmount);
        }

        public void SpendAmmo(int amount = 1)
        {
            if (CurrentAmmoAmount <= 0) return;
            CurrentAmmoAmount -= amount;
            OnAmmoChange?.Invoke(CurrentAmmoAmount);
        }
        
        [ContextMenu("Grant 90 exp")]
        public void GrantExperience90()
        {
            GrantExperience(90);
        }

        protected void Update()
        {
            Move();
        }

        private void Move()
        {
            if (!Alive) return;
            
            if (MovementDirection != Vector2.zero)
            {
                FacingDirection = MovementDirection;
                // var movementSpeed = Mathf.Clamp(_movementStat.Value, 0, Mathf.Infinity);
                var movementSpeed = Mathf.Clamp(_movementStat.Value, 0, Mathf.Infinity);

                var movementAmount = movementSpeed * Time.deltaTime;
                var delta = movementAmount * MovementDirection;
                transform.Translate(delta);
            }

            var flipX = FacingDirection.x >= 0 ? 1 : -1;
            graphics.transform.localScale = new Vector3(flipX, 1, 1);
            animator.SetBool(Running, MovementDirection != Vector2.zero);
        }
    }
}