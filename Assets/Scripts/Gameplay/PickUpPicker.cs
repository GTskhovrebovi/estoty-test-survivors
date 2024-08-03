using UnityEngine;

namespace Gameplay
{
    public class PickUpPicker : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private StatType pickUpStatType;
        [SerializeField] private CircleCollider2D pickUpTriggerCollider;
        
        private Stat _pickUpStat;

        private void Start()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            _pickUpStat = character.CharacterStats.GetStat(pickUpStatType);
            UpdateRange();
            _pickUpStat.OnStatChanged += UpdateRange;
        }

        private void UpdateRange()
        {
            pickUpTriggerCollider.radius = _pickUpStat.Value / 2;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var pickUp = other.GetComponent<PickUp>();
            if (pickUp == null) return;
            pickUp.Pick(character);
        }

        private void OnDestroy()
        {
            _pickUpStat.OnStatChanged -= UpdateRange;
        }
    }
}