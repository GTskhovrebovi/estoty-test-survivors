using Gameplay.WeaponSystem;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GameplayContext : MonoBehaviour
    {
        public WeaponObjectFactory WeaponObjectFactory { get; private set; }
        
        [Inject]
        public void Construct(WeaponObjectFactory weaponObjectFactory)
        {
            WeaponObjectFactory = weaponObjectFactory;
        }
    }
}