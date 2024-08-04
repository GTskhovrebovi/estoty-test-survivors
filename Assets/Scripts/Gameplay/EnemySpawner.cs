using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        private Settings _settings;
        private Character _player;
        private float _currentSpawnInterval;
        private float _timeSinceLastSpawn;
        private bool _spawning;
        private CharacterFactory _characterFactory;

        [Inject]
        public void Construct(CharacterFactory characterFactory, Settings settings)
        {
            _characterFactory = characterFactory;
            _settings = settings;
        }
        
        public void Initialize(Character player)
        {
            _player = player;
        }

        public void StartSpawning()
        {
            if (_player == null) return;
            if (_spawning) return;
            
            _currentSpawnInterval = _settings.initialSpawnInterval;
            _timeSinceLastSpawn = 0f;
            _spawning = true;
        }

        public void StopSpawning()
        {
            _spawning = false;
        }

        private void Update()
        {
            CheckForSpawn();
        }
        
        private void CheckForSpawn()
        {
            if (!_spawning) return;

            _timeSinceLastSpawn += Time.deltaTime;
            if (_timeSinceLastSpawn >= _currentSpawnInterval)
            {
                Spawn(_player);
                _timeSinceLastSpawn = 0f;
                _currentSpawnInterval = Mathf.Max(_currentSpawnInterval - _settings.spawnIntervalDecrease, _settings.minimumSpawnInterval);
            }
        }
        
        public void Spawn(Character player)
        {
            if (_settings.enemyTypes == null || _settings.enemyTypes.Count == 0) return;

            var randomIndex = Random.Range(0, _settings.enemyTypes.Count);
            var randomEnemy = _settings.enemyTypes[randomIndex];
            
            var angle = Random.Range(0f, Mathf.PI * 2);
            var x = _settings.spawnRadius * Mathf.Cos(angle);
            var y = _settings.spawnRadius * Mathf.Sin(angle);
            var spawnPosition = new Vector3(x, y, 0);
            
            var spawnedEnemy = _characterFactory.Spawn(randomEnemy, _settings.enemyTeam, player.transform.position + spawnPosition);
            if (spawnedEnemy.TryGetComponent<EnemyAIController>(out var characterBehaviorController))
            {
                characterBehaviorController.SetTarget(_player);
            }
        }
        
        [Serializable]
        public class Settings
        {
            
            
            [SerializeField] public List<CharacterData> enemyTypes;
            [SerializeField] public float spawnRadius;
            [SerializeField] public Team enemyTeam;
            [SerializeField] public float initialSpawnInterval = 5f;
            [SerializeField] public float spawnIntervalDecrease = 0.1f;
            [SerializeField] public float minimumSpawnInterval = 1f;
        }
    }
}