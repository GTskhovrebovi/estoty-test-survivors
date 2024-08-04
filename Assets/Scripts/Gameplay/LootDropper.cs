using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    [RequireComponent(typeof(Character))]
    public class LootDropper : MonoBehaviour
    {
        [SerializeField] private List<PickUp> possibleLoot;

        private Character _character;
        private PickUpFactory _pickUpFactory;

        [Inject]
        public void Construct(PickUpFactory pickUpFactory)
        {
            _pickUpFactory = pickUpFactory;
        }

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void OnEnable()
        {
            _character.OnDeath += HandleCharacterDeath;
        }

        private void OnDisable()
        {
            _character.OnDeath -= HandleCharacterDeath;
        }

        private void HandleCharacterDeath()
        {
            if (possibleLoot != null)
            {
                DropLoot();
            }
        }

        private void DropLoot()
        {
            //TODO: implement drop chances 
            var lootToDrop = new List<PickUp>();
            foreach (var loot in possibleLoot)
            {
                if (Random.value > 0.5f) lootToDrop.Add(loot);
            }

            var numberOfItems = lootToDrop.Count;
            var lootDropRadius = Mathf.Pow(numberOfItems, 0.5f) - 1;

            foreach (var pickUp in lootToDrop)
            {
                var dropPosition = Random.insideUnitCircle.normalized * (Random.value * lootDropRadius);
                var pickUpInstance = _pickUpFactory.GetPickUp(pickUp, transform.position);
                pickUpInstance.Drop(dropPosition);
            }
        }
    }
}