using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Projectile : WeaponObject
    {
        [SerializeField] protected float collisionRadius;

        private LayerMask _hitLayerMask;
        private float _travelSpeed;
        
        private Transform _transform;
        private Vector2 _previousPosition;
        private float _distanceTravelled;
        private float _travelDistanceLimit;
        
        private List<WeaponActionOnCharacter> _onHitActions;
        private RaycastHit2D[] _hits = new RaycastHit2D[20];
        
        protected override void Awake()
        {
            base.Awake();
            _transform = transform;
        }
        
        public void Initialize(
            Character owner,
            CharacterStats characterStats,
            Team team,
            Weapon weapon,
            List<WeaponActionOnCharacter> onHitActions,
            float travelSpeed,
            float travelDistanceLimit,
            LayerMask hitLayerMask)
        {
            Initialize(owner, characterStats, team, weapon);
            _travelSpeed = travelSpeed;
            _hitLayerMask = hitLayerMask;
            _onHitActions = onHitActions;
            _travelDistanceLimit = travelDistanceLimit;
            _distanceTravelled = 0;

        }
        
        public override void StartBehavior()
        {
            base.StartBehavior();
            _previousPosition = transform.position;
        }
        
        protected override void Update()
        {
            base.Update();
            if (!IsActive) return;

            CheckTravelDistanceLimit();
            Move();
            CheckCollision();
        }
        
        private void CheckTravelDistanceLimit()
        {
            if (_distanceTravelled > _travelDistanceLimit)
                End();
        }

        private void Move()
        {
            var movement = _transform.right * (_travelSpeed * Time.deltaTime);
            _distanceTravelled += movement.magnitude;
            _transform.position += movement;
        }

        private void ExecuteOnHitActions(Character character)
        {
            foreach (var onHitAction in _onHitActions) 
                onHitAction.TryExecute(new WeaponActionExecutionData(Owner, CharacterStats, Team, Weapon), Weapon.Container, character);
        }
        
        protected override void End()
        {
            LifeTime = InitialLifeTime;
            base.End();
        }
        
        private void CheckCollision()
        {
            Vector2 currentPosition = transform.position;
            var direction = currentPosition - _previousPosition;
            var distance = direction.magnitude;

            if (!(distance > 0)) return;
            
            var size = Physics2D.CircleCastNonAlloc(_previousPosition, collisionRadius * transform.localScale.x, direction, _hits, distance, _hitLayerMask);
            Debug.DrawLine(_previousPosition, (Vector3)_previousPosition + (Vector3)(direction * distance), Color.green);

            _previousPosition = currentPosition;
            
            for (var i = 0; i < size; i++)
            {
                var hit = _hits[i];
                if (IsActive)
                {
                    HandleCollision(hit.collider);
                    break;
                }
            }
        }
        
        private void HandleCollision(Collider2D other)
        {
            if (!IsActive) return;
            if (_hitLayerMask != (_hitLayerMask | (1 << other.gameObject.layer))) return;
            
            var character = other.gameObject.GetComponent<Character>();
            if (character == null) return;
            if (!Team.IsEnemy(character.Team)) return;

            ExecuteOnHitActions(character);
            End();
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, collisionRadius);
        }
    }
}