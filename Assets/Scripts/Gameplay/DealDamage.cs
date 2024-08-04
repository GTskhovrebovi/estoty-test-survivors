using System;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    [Serializable]
    [CreateAssetMenu(fileName = "Deal Damage", menuName = "Weapon Actions/Deal Damage", order = 1)]
    public class DealDamage : WeaponActionOnCharacter
    {
        [SerializeField] protected Variable damageVariable;

        protected override void Execute(WeaponActionExecutionData data, DiContainer container, Character character)
        {
            if (!data.Team.IsEnemy(character.Team)) return;
            
            var damage = damageVariable.Value(data.CharacterStats);
            var damageEventArgs = new DamageEventArgs(data.Owner, character, damage);

            character.GetHit(damageEventArgs);
        }
    }
}