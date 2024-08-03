using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Character))]
    public class LootDropper : MonoBehaviour
    {
        [SerializeField] private List<PickUp> possibleLoot;

        private Character _character;
        
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
        
        [ContextMenu("DropLoot")]
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
                var pickUpInstance = FindObjectOfType<PickUpFactory>().GetPickUp(pickUp, transform.position);
                pickUpInstance.Drop(dropPosition);
            }
        }
    }
}