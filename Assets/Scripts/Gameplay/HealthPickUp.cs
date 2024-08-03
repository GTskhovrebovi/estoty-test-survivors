using UnityEngine;

namespace Gameplay
{
    public class HealthPickUp : PickUp
    {
        [field: SerializeField] public float Amount { get; private set; }
        [field: SerializeField] public HealthChangeType HealType { get; private set; }

        protected override void Consume(Character character)
        {
            base.Consume(character);
            character.Health.Heal(Amount, HealType);
        }
    }
}