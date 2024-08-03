using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    [CreateAssetMenu(fileName = "Deal Damage", menuName = "Weapon Actions/Deal Damage", order = 1)]
    public class DealDamage : WeaponActionOnCharacter
    {
        [SerializeField] protected Variable damageVariable;

        public override void Execute(WeaponActionExecutionData data, Character character)
        {
            if (!data.Team.IsEnemy(character.Team)) return;
            
            var damage = damageVariable.Value(data.CharacterStats);
            var damageEventArgs = new DamageEventArgs(data.Owner, character, damage);

            character.GetHit(damageEventArgs);
        }
    }
}