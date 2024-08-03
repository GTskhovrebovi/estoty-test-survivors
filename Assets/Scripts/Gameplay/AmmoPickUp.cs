using UnityEngine;

namespace Gameplay
{
    public class AmmoPickUp : PickUp
    {
        [field: SerializeField] public int Amount { get; private set; }

        protected override void Consume(Character character)
        {
            base.Consume(character);
            character.GrantAmmo(Amount);
        }
    }
}