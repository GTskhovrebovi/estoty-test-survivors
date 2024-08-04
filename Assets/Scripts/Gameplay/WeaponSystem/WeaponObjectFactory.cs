using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.WeaponSystem
{
    public class WeaponObjectFactory : MonoBehaviour
    {
        private Dictionary<WeaponObject, ObjectPool<WeaponObject>> _weaponObjectPools = new();

        private void CreatePoolForWeaponObject(WeaponObject weaponObject)
        {
            _weaponObjectPools.Add(weaponObject, new ObjectPool<WeaponObject>(
                () =>
                {
                    var instantiatedWeaponObject = Instantiate(weaponObject);
                    instantiatedWeaponObject.EndAction = i =>
                    {
                        if (_weaponObjectPools.ContainsKey(weaponObject))
                            _weaponObjectPools[weaponObject].Release(i);
                    };
                    return instantiatedWeaponObject;
                },
                i => { i.gameObject.SetActive(true); },
                i => { i.gameObject.SetActive(false); },
                i => { Destroy(i.gameObject); },
                true,
                400,
                400
            ));
        }

        public WeaponObject GetWeaponObject(WeaponObject weaponObject)
        {
            if (!_weaponObjectPools.ContainsKey(weaponObject)) CreatePoolForWeaponObject(weaponObject);

            return _weaponObjectPools[weaponObject].Get();
        }
    }
}