using System;
using UnityEngine;

namespace Gameplay
{
    public class Health : MonoBehaviour
    {
        private float _totalHealth;
        private float _currentHealth;
        private float _lastRegenerationTick;
    
        private Stat _healthStat;
        private Stat _regenerationStat;
    
        public float TotalHealth => _totalHealth;
        public float CurrentHealth => _currentHealth;
        public float HealthFraction => _currentHealth / _totalHealth;
    
        public event Action OnHealthChanged;
        private Action _healthReachZeroCallback;
    
        private void Awake()
        {
            _lastRegenerationTick = Time.time;
        }

        public void Initialize(Stat healthStat, Stat regenerationStat, Action healthReachZeroCallback)
        {
            if (_healthStat != null) _healthStat.OnStatChanged -= ReactOnHealthStatChange;
        
            _healthStat = healthStat;
            _regenerationStat = regenerationStat;
            _totalHealth = _healthStat.Value;
            _currentHealth = _totalHealth;
            _healthStat.OnStatChanged += ReactOnHealthStatChange;
            _healthReachZeroCallback = healthReachZeroCallback;
        }

        public void GetHit(float damage)
        {
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            OnHealthChanged?.Invoke();
            if (_currentHealth == 0) _healthReachZeroCallback?.Invoke();
        }

        private void Update()
        {
            var deltaTime = Time.time - _lastRegenerationTick;
            _lastRegenerationTick = Time.time;
        
            _currentHealth = Mathf.Min(_currentHealth + _regenerationStat.Value * deltaTime, _totalHealth);
        }

        public void Heal(float amount, HealthChangeType healType)
        {
            //Debug.Log($" {healType} Healing: {amount}".Colorize(Color.green));
            switch (healType)
            {
                case HealthChangeType.Flat:
                    _currentHealth = Mathf.Min(_totalHealth, _currentHealth + amount);
                    OnHealthChanged?.Invoke();
                    break;
                case HealthChangeType.Percentage:
                    _currentHealth = Mathf.Min(_totalHealth, _currentHealth + _totalHealth * amount);
                    OnHealthChanged?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(healType), healType, null);
            }

            //OnHealTaken?.Invoke(amount);
        }
    
        private void ReactOnHealthStatChange()
        {
            var fraction = HealthFraction;
            _totalHealth = _healthStat.Value;
            _currentHealth = _totalHealth * fraction;
            //OnTotalHealthChanged?.Invoke();
        }

        public void MakeHealthFull()
        {
            _currentHealth = _totalHealth;
            OnHealthChanged?.Invoke();
        }
    }

    public enum HealthChangeType
    {
        Flat,
        Percentage
    }
}