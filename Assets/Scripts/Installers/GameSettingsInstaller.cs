using System;
using System.Collections.Generic;
using Gameplay;
using Gameplay.UpgradeSystem;
using Gameplay.WeaponSystem;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(menuName = "Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [field: SerializeField] public EnemySpawner.Settings EnemySpawnerSettings { get; private set; }
        [field: SerializeField] public WeaponLibrary WeaponLibrary { get; private set; }
        [field: SerializeField] public UpgradeLibrary UpgradeLibrary { get; private set; }

        public override void InstallBindings()
        {
            Container.BindInstance(EnemySpawnerSettings).AsSingle();
            Container.BindInstance(WeaponLibrary).AsSingle();
            Container.BindInstance(UpgradeLibrary).AsSingle();
        }
    }

    [Serializable]
    public class WeaponLibrary
    {
        [SerializeField] public List<WeaponData> Weapons;
    }

    [Serializable]
    public class UpgradeLibrary
    {
        [SerializeField] public List<Upgrade> Upgrades;
    }
}