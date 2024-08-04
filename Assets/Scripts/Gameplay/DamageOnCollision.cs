using UnityEngine;

namespace Gameplay
{
    public class DamageOnCollision : MonoBehaviour
    {
        [SerializeField] private StatType damageStat;
        [SerializeField] private float cooldown = 0.4f;

        private Character _character;
        private float _lastHitTime = Mathf.NegativeInfinity;

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (Time.time < _lastHitTime + cooldown) return;

            var otherCharacter = other.transform.GetComponentInParent<Character>();
            if (otherCharacter == null) return;

            if (_character.Team.IsEnemy(otherCharacter.Team))
            {
                Hit(otherCharacter);
            }
        }

        private void Hit(Character otherCharacter)
        {
            var damageAmount = _character.CharacterStats.GetStat(damageStat).Value;
            var damageEventArgs = new DamageEventArgs(_character, otherCharacter, damageAmount);

            otherCharacter.GetHit(damageEventArgs);
            _lastHitTime = Time.time;
        }
    }
}