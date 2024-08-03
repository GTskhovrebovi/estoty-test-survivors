using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class CharacterFactory : MonoBehaviour
    {
        private Dictionary<CharacterData, ObjectPool<Character>> _characterObjectPools = new();
        
        private void CreatePoolForCharacter(CharacterData characterData)
        {
            _characterObjectPools.Add(characterData, new ObjectPool<Character>(
                () =>
                {
                    var instantiatedCharacter = Object.Instantiate(characterData.CharacterPrefab);
                    instantiatedCharacter.OnEndExistence += i =>_characterObjectPools[characterData].Release(i);
                    return instantiatedCharacter;
                },
                i =>
                {
                    i.gameObject.SetActive(true);
                },
                i =>
                {
                    i.gameObject.SetActive(false);
                },
                i => Object.Destroy(i.gameObject),
                true,
                400
            ));
        }
        
        public Character Spawn(CharacterData characterData, Team team, Vector3 spawnPosition)
        {
            if (!_characterObjectPools.ContainsKey(characterData)) CreatePoolForCharacter(characterData);

            _characterObjectPools[characterData].Get(out var pooledCharacter);
            SceneManager.MoveGameObjectToScene(pooledCharacter.gameObject, SceneManager.GetActiveScene());
            pooledCharacter.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            pooledCharacter.RigidBody.position = spawnPosition;
            pooledCharacter.Initialize(characterData.BaseStats, team, characterData.StartingWeapons);
            
            return pooledCharacter;
        }
    }
}