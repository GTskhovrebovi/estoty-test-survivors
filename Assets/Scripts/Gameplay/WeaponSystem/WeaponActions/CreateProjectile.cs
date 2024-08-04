using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Gameplay.WeaponSystem.WeaponActions
{
    [Serializable]
    [CreateAssetMenu(fileName = "Create Projectile", menuName = "Gameplay/Weapon Actions/Create Projectile", order = 1)]
    public class CreateProjectile : WeaponAction
    {
        [SerializeField] protected Projectile projectilePrefab;
        [SerializeField] protected Variable travelSpeed;
        [SerializeField] protected Variable travelDistance;
        [SerializeField] protected List<WeaponActionOnCharacter> onHitActions = new();

        protected override void Execute(WeaponActionExecutionData data, DiContainer _container)
        {
            if (!data.Owner.HasAmmo) return;
            if (data.Weapon.Target == null) return;

            data.Owner.SpendAmmo();

            var spawnPoint = data.Weapon.SpawnPoint;
            var weaponObjectFactory = _container.Resolve<WeaponObjectFactory>();
            var instantiatedProjectile = (Projectile)weaponObjectFactory.GetWeaponObject(projectilePrefab);
            instantiatedProjectile.transform.position = spawnPoint.position;
            instantiatedProjectile.transform.right = spawnPoint.right;

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