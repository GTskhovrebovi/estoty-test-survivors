using System;
using UnityEngine;

namespace Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private CharacterData enemyData;
        [SerializeField] private Team enemyTeam;
        [SerializeField] private Transform spawnPoint;

        private Character _player;
        
        public void Initialize(Character player)
        {
            _player = player;
            Spawn();
            Spawn();
            Spawn();
            Spawn();
            Spawn();
            Spawn();
            Spawn();
        }
        
        // private void Start()
        // {
        //     Spawn();
        // }

        public void Spawn()
        {
            var spawnedEnemy = FindObjectOfType<CharacterFactory>().Spawn(enemyData, enemyTeam, spawnPoint.position);
            if (spawnedEnemy.TryGetComponent<EnemyAIController>(out var characterBehaviorController))
            {
                characterBehaviorController.SetTarget(_player);
            }
        }
    }
}