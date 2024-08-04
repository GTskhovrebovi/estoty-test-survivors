using Gameplay.UpgradeSystem;
using UnityEngine;

namespace Gameplay
{
    public class UpgradePickUp : PickUp
    {
        [field: SerializeField] public Upgrade Upgrade { get; private set; }

        protected override void Consume(Character character)
        {
            base.Consume(character);
            if (character.TryGetComponent<UpgradeHolder>(out var weaponUser))
            {
                weaponUser.ApplyUpgrade(Upgrade);
            }
        }
    }
}