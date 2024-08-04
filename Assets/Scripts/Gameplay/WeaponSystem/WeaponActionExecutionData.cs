using UnityEngine;

namespace Gameplay.WeaponSystem
{
    public class WeaponActionExecutionData
    {
        public Character Owner { get; private set; }
        public CharacterStats CharacterStats { get; private set; }
        public Team Team { get; private set; }

        public Weapon Weapon { get; private set; }

        public WeaponActionExecutionData(Character owner, CharacterStats characterStats, Team team, Weapon weapon)
        {
            Owner = owner;
            CharacterStats = characterStats;
            Team = team;
            Weapon = weapon;
        }
    }
}