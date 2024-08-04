using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootManager : MonoBehaviour
{
    [SerializeField] private int gameplaySceneIndex;

    private async void Start()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        await SceneManager.LoadSceneAsync(gameplaySceneIndex);
    }
}