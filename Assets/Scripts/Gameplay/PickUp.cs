using System;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
    [ExecuteInEditMode]
    public class PickUp : MonoBehaviour
    {
        [SerializeField] private float pickUpAnimationDuration;
        [SerializeField] private float dropAnimationDuration;
        [SerializeField] private AnimationCurve pickUpCurve;
        [SerializeField] private AnimationCurve dropCurve;
        [SerializeField] private AnimationCurve dropHeightCurve;
        [SerializeField] private float dropHeight;
        [SerializeField] private AnimationCurve pickUpAfterDropCurve;
        
        private Collider2D _collider;
        private bool _inDropAnimation;
        private bool _shouldBePickedAfterDrop;
        private Character _targetCharacter;
        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private float _startTime;
        
        public bool CanBePicked { get; private set; }
        public Action<PickUp> EndAction { get; set; }

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            CanBePicked = true;
        }

        public void Pick(Character character)
        {
            CanBePicked = false;
            _targetCharacter = character;

            if (!_inDropAnimation)
                StartPickAnimation();
            else
                _shouldBePickedAfterDrop = true;
        }

        private void StartPickAnimation(bool continuedFromDrop = false)
        {
            _collider.enabled = false;
            var curve = continuedFromDrop ? pickUpAfterDropCurve : pickUpCurve;
            var duration = continuedFromDrop ? dropAnimationDuration : pickUpAnimationDuration;

            var distance = Vector2.Distance(transform.position, _targetCharacter.Center);
            duration += 0.05f * distance;

            _startPosition = transform.position;
            _startTime = Time.time;

            StartCoroutine(PickUpAnimationCoroutine(duration, curve));
        }

        private IEnumerator PickUpAnimationCoroutine(float duration, AnimationCurve curve)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                float curveValue = curve.Evaluate(t);
                transform.position = Vector2.LerpUnclamped(_startPosition, _targetCharacter.Center, curveValue);
                elapsedTime = Time.time - _startTime;
                yield return null;
            }

            Consume(_targetCharacter);
        }

        protected virtual void Consume(Character character)
        {
            End();
        }
        
        private void End()
        {
            gameObject.SetActive(false);
            _inDropAnimation = false;
            _shouldBePickedAfterDrop = false;
            _targetCharacter = null;
            _collider.enabled = true;
            EndAction?.Invoke(this);
        }

        public void Drop(Vector2 dropPosition)
        {
            _inDropAnimation = true;
            _startPosition = transform.position;
            _endPosition = _startPosition + dropPosition;
            _startTime = Time.time;

            StartCoroutine(DropAnimationCoroutine(dropAnimationDuration, dropCurve));
        }

        private IEnumerator DropAnimationCoroutine(float duration, AnimationCurve curve)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                float curveValue = curve.Evaluate(t);
                float heightCurveValue = dropHeightCurve.Evaluate(t);
                
                transform.position = Vector2.LerpUnclamped(_startPosition, _endPosition, curveValue) + new Vector2(0, heightCurveValue * dropHeight);
                elapsedTime = Time.time - _startTime;
                yield return null;
            }

            transform.position = _endPosition;
            
            HandleDropEnd();
        }

        private void HandleDropEnd()
        {
            _inDropAnimation = false;
            if (_shouldBePickedAfterDrop)
                StartPickAnimation(true);
        }
    }
}
