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
        private Action _deathCallback;
        private bool _alive;
        private void Awake()
        {
            _lastRegenerationTick = Time.time;
        }

        public void Initialize(Stat healthStat, Stat regenerationStat, Action deathCallback)
        {
            if (_healthStat != null) _healthStat.OnStatChanged -= ReactOnHealthStatChange;
        
            _healthStat = healthStat;
            _regenerationStat = regenerationStat;
            _totalHealth = _healthStat.Value;
            _currentHealth = _totalHealth;
            _healthStat.OnStatChanged += ReactOnHealthStatChange;
            _deathCallback = deathCallback;
            _alive = true;
        }

        public void GetHit(float damage)
        {
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            OnHealthChanged?.Invoke();
            
            if (_currentHealth == 0)
            {
                _alive = false;
                _deathCallback?.Invoke();
            }
        }

        private void Update()
        {
            if (!_alive) return;
            var deltaTime = Time.time - _lastRegenerationTick;
            _lastRegenerationTick = Time.time;
            _currentHealth = Mathf.Min(_currentHealth + _regenerationStat.Value * deltaTime, _totalHealth);
            OnHealthChanged?.Invoke();
        }

        public void Heal(float amount, HealthChangeType healType)
        {
            if (!_alive) return;
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

        }
    
        private void ReactOnHealthStatChange()
        {
            if (!_alive) return;
            var fraction = HealthFraction;
            _totalHealth = _healthStat.Value;
            _currentHealth = _totalHealth * fraction;
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