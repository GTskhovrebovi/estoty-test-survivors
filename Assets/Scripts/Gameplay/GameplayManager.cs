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
    
        public Character Player { get; private set; }

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            Player = FindObjectOfType<CharacterFactory>().Spawn(playerData, playerTeam, spawnPoint.position);
            gameUI.Initialize(Player);
            cameraController.Initialize(Player);
            enemySpawner.Initialize(Player);
        }
    }
}