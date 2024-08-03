using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class UpgradeInfoPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text upgradeTypeText;
        [SerializeField] private TMP_Text upgradeDescriptionText;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private float displayDuration = 2f;

        private CanvasGroup _canvasGroup;
        private CancellationTokenSource _cts;
        private Tween _activeTween;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            _canvasGroup.alpha = 0f;
        }

        public void Show(AddUpgrade addUpgrade)
        {
            upgradeTypeText.text = "New Upgrade";
            upgradeDescriptionText.text = addUpgrade.Upgrade.UpgradeDescription;
            StartAnimation();
        }
        
        public void Show(AddWeapon addWeapon)
        {
            upgradeTypeText.text = "New Weapon";
            upgradeDescriptionText.text = addWeapon.WeaponData.WeaponName;
            StartAnimation();
        }

        private void StartAnimation()
        {
            gameObject.SetActive(true);
            
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            
            if (_activeTween != null && _activeTween.IsActive())
            {
                _activeTween.Kill();
            }
            
            ShowPopupAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid ShowPopupAsync(CancellationToken token)
        {
            _activeTween = _canvasGroup.DOFade(1f, fadeDuration);
            await _activeTween.ToUniTask(cancellationToken: token);

            await UniTask.Delay((int)(displayDuration * 1000), cancellationToken: token);

            _activeTween = _canvasGroup.DOFade(0f, fadeDuration);
            await _activeTween.ToUniTask(cancellationToken: token);
            
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
        }
    }
}