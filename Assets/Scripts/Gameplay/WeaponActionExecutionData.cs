using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class WeaponActionExecutionData
    {
        public Character Owner { get; private set; }
        public CharacterStats CharacterStats { get; private set; }
        public Team Team { get; private set; }

        public Weapon Weapon { get; private set; } = null;
        public Context context;
        public WeaponActionExecutionData(Character owner, CharacterStats characterStats, Team team, Weapon weapon)
        {
            if (owner == null) Debug.Log("Action Executed with null owner");
            Owner = owner;
            CharacterStats = characterStats;
            Team = team;
            Weapon = weapon;
        }
    }
}