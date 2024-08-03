using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private CharacterData playerData;
        [SerializeField] private GameUI gameUI;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] protected CameraController cameraController;
        [SerializeField] protected Transform spawnPoint;
        [SerializeField] protected Team playerTeam;

        private Character _player;

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            _player = FindObjectOfType<CharacterFactory>().Spawn(playerData, playerTeam, spawnPoint.position);
            gameUI.Initialize(_player);
            cameraController.Initialize(_player);
            enemySpawner.Initialize(_player);
            enemySpawner.StartSpawning();

            _player.OnDeath += HandlePlayerDeath;
        }

        private void HandlePlayerDeath()
        {
            _player.OnDeath -= HandlePlayerDeath;
            
            enemySpawner.StopSpawning();
        }
    }
}