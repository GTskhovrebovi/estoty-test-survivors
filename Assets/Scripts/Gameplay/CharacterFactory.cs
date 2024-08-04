using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Gameplay
{
    public class CharacterFactory : MonoBehaviour
    {
        private Dictionary<CharacterData, ObjectPool<Character>> _characterObjectPools = new();
        private Character.Factory _characterFactory;

        [Inject]
        public void Construct(Character.Factory characterFactory)
        {
            _characterFactory = characterFactory;
        }

        private void CreatePoolForCharacter(CharacterData characterData)
        {
            _characterObjectPools.Add(characterData, new ObjectPool<Character>(
                () =>
                {
                    var instantiatedCharacter = _characterFactory.Create(characterData.CharacterPrefab);
                    instantiatedCharacter.OnEndExistence += i => _characterObjectPools[characterData].Release(i);
                    return instantiatedCharacter;
                },
                i => { i.gameObject.SetActive(true); },
                i => { i.gameObject.SetActive(false); },
                i => Destroy(i.gameObject),
                true,
                400
            ));
        }

        public Character Spawn(CharacterData characterData, Team team, Vector3 spawnPosition)
        {
            if (!_characterObjectPools.ContainsKey(characterData)) CreatePoolForCharacter(characterData);

            _characterObjectPools[characterData].Get(out var pooledCharacter);
            pooledCharacter.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            pooledCharacter.RigidBody.position = spawnPosition;
            pooledCharacter.Initialize(characterData.BaseStats, team, characterData.StartingWeapons);

            return pooledCharacter;
        }
    }
}