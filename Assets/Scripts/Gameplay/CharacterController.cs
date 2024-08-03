using UnityEngine;

namespace Gameplay
{
    public class CharacterController : MonoBehaviour
    {
        private Character _character;
        private float _inputHorizontal;
        private float _inputVertical;
        public string horizontalAxis = "Horizontal";
        public string verticalAxis = "Vertical";
    
        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void Update()
        {
            _inputHorizontal = SimpleInput.GetAxis( horizontalAxis );
            _inputVertical = SimpleInput.GetAxis( verticalAxis );

            var movementDirection = new Vector2(_inputHorizontal, _inputVertical).normalized;
            _character.MovementDirection = movementDirection;
        }
    }
}