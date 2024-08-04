using Gameplay;
using Gameplay.WeaponSystem;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private CharacterFactory _characterFactory;
        [SerializeField] private WeaponObjectFactory _weaponObjectFactory;
        [SerializeField] private PickUpFactory _pickUpFactory;
        [SerializeField] private GameplayContext _gameplayContext;

        public override void InstallBindings()
        {
            Container.Bind<CharacterFactory>().FromInstance(_characterFactory).AsSingle();
            Container.Bind<WeaponObjectFactory>().FromInstance(_weaponObjectFactory).AsSingle();
            Container.Bind<PickUpFactory>().FromInstance(_pickUpFactory).AsSingle();
            Container.Bind<GameplayContext>().FromInstance(_gameplayContext).AsSingle();
            Container.BindFactory<Character, Character.Factory>().FromComponentInHierarchy();
            Container.BindFactory<Character, WeaponUser, WeaponSlot, WeaponSlot.Factory>().AsSingle();
            Container.BindFactory<Weapon, Weapon.Factory>().AsSingle();
        }
    }
}