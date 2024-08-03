using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<CharacterData> enemyTypes;
        [SerializeField] private float spawnRadius;
        [SerializeField] private Team enemyTeam;
        [SerializeField] private float initialSpawnInterval = 5f;
        [SerializeField] private float spawnIntervalDecrease = 0.1f;
        [SerializeField] private float minimumSpawnInterval = 1f;

        private Character _player;
        private float _currentSpawnInterval;
        private float _timeSinceLastSpawn;
        private bool _spawning;

        public void Initialize(Character player)
        {
            _player = player;
        }

        public void StartSpawning()
        {
            if (_player == null) return;
            if (_spawning) return;
            
            _currentSpawnInterval = initialSpawnInterval;
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
                _currentSpawnInterval = Mathf.Max(_currentSpawnInterval - spawnIntervalDecrease, minimumSpawnInterval);
            }
        }
        

        
        public void Spawn(Character player)
        {
            if (enemyTypes == null || enemyTypes.Count == 0) return;

            var randomIndex = Random.Range(0, enemyTypes.Count);
            var randomEnemy = enemyTypes[randomIndex];
            
            var angle = Random.Range(0f, Mathf.PI * 2);
            var x = spawnRadius * Mathf.Cos(angle);
            var y = spawnRadius * Mathf.Sin(angle);
            var spawnPosition = new Vector3(x, y, 0);
            
            var spawnedEnemy = FindObjectOfType<CharacterFactory>()
                .Spawn(randomEnemy, enemyTeam, player.transform.position + spawnPosition);
            if (spawnedEnemy.TryGetComponent<EnemyAIController>(out var characterBehaviorController))
            {
                characterBehaviorController.SetTarget(_player);
            }
        }
    }
}