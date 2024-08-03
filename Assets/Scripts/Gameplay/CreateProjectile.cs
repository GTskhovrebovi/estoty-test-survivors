using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    [CreateAssetMenu(fileName = "Create Projectile", menuName = "Weapon Actions/Create Projectile", order = 1)]
    public class CreateProjectile : WeaponAction
    {
        [SerializeField] protected Projectile projectilePrefab;
        [SerializeField] protected Variable travelSpeed;
        [SerializeField] protected Variable travelDistance;
        [SerializeField] protected List<WeaponActionOnCharacter> onHitActions = new();
        
        protected override void Execute(WeaponActionExecutionData data)
        {
            // if (!data.Owner.HasAmmo) return;
            var spawnPoint = data.Weapon.SpawnPoint;
            
            var instantiatedProjectile = (Projectile) FindObjectOfType<WeaponObjectFactory>().GetWeaponObject(projectilePrefab);
            instantiatedProjectile.transform.position = spawnPoint.position;
            instantiatedProjectile.transform.right = spawnPoint.right;
            // data.Owner.SpendAmmo();
                
            instantiatedProjectile.Initialize(
                data.Owner,
                data.CharacterStats,
                data.Team,
                data.Weapon,
                onHitActions,
                travelSpeed.Value(data.CharacterStats),
                travelDistance.Value(data.CharacterStats),
                Layers.CharacterLayerMask);
        }
    }
}