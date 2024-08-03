using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class PickUpFactory : MonoBehaviour
    {
        private Dictionary<PickUp, ObjectPool<PickUp>> _pickUpPools = new();
        
        private void CreatePoolForPickUp(PickUp pickUp)
        {
            _pickUpPools.Add(pickUp, new ObjectPool<PickUp>(
                () =>
                {
                    var pickUpInstance = Instantiate(pickUp);
                    pickUpInstance.EndAction = i =>_pickUpPools[pickUp].Release(i);
                    return pickUpInstance;
                },
                i =>
                {
                    i.gameObject.SetActive(true);
                },
                i =>
                {
                    i.gameObject.SetActive(false);
                },
                i => Destroy(i.gameObject),
                true,
                10
            ));
        }

        public PickUp GetPickUp(PickUp pickUp, Vector3 spawnPosition)
        {
            if (!_pickUpPools.ContainsKey(pickUp)) CreatePoolForPickUp(pickUp);
            
            var pooledPickUp = _pickUpPools[pickUp].Get();
            pooledPickUp.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            return pooledPickUp;
        }
    }
}