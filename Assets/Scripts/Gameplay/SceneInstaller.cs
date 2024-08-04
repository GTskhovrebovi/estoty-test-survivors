using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private CharacterFactory _characterFactory;
        [SerializeField] private WeaponObjectFactory _weaponObjectFactory;
        [SerializeField] private PickUpFactory _pickUpFactory;

        public override void InstallBindings()
        {
            Container.Bind<CharacterFactory>().FromInstance(_characterFactory).AsSingle();
            Container.Bind<WeaponObjectFactory>().FromInstance(_weaponObjectFactory).AsSingle();
            Container.Bind<PickUpFactory>().FromInstance(_pickUpFactory).AsSingle();
            
            Container.BindFactory<Character, Character.Factory>().AsSingle();
            Container.BindFactory<Character, WeaponUser, WeaponSlot, WeaponSlot.Factory>().AsSingle();
            Container.BindFactory<Weapon, Weapon.Factory>().AsSingle();
        }
    }
}