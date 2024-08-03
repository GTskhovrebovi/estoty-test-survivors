using UnityEngine;

namespace Gameplay
{
    public class ExperiencePickUp : PickUp
    {
        [field: SerializeField] public int Amount { get; private set; }

        protected override void Consume(Character character)
        {
            base.Consume(character);
            character.GrantExperience(Amount);
        }
    }
}