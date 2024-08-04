using System;
using UnityEngine;

namespace Gameplay
{
    public class EnemyAIController : MonoBehaviour
    {
        private Character _character;
        private Character _targetCharacter;

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        public void SetTarget(Character character)
        {
            _targetCharacter = character;
        }

        private void Update()
        {
            if (_targetCharacter == null)
            {
                _character.MovementDirection = Vector2.zero;
                return;
            }

            if (!_targetCharacter.Alive)
            {
                _character.MovementDirection = Vector2.zero;
            }
            else
            {
                _character.MovementDirection =
                    (_targetCharacter.transform.position - _character.transform.position).normalized;
            }
        }
    }
}