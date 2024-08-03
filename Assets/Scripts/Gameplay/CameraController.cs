using Cinemachine;
using UnityEngine;

namespace Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        
        public void Initialize(Character player)
        {
            virtualCamera.Follow = player.transform;
        }
    }
}