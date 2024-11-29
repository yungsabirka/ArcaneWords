using System.Threading.Tasks;
using ArcaneWords.Scripts.Game.GameRoot.UI;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace ArcaneWords.Scripts.Game.Gameplay.Popups
{
    public class UICoinPopupView : MonoBehaviour
    {
        private UICoinsView _coinsView;

        [Inject]
        private void Construct(UICoinsView coinsView)
        {
            _coinsView = coinsView;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public async Task MoveToCoinsView(Vector3 startPosition, float duration = 1)
        {
            transform.DOKill();
            transform.position = startPosition;
            var targetPosition = Vector3.Lerp(startPosition, _coinsView.transform.position, 0.8f);
            var rotateTween = transform.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360);
            var tsc = new TaskCompletionSource<bool>();

            transform.DOScale(Vector3.one * 1.5f, duration / 2)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => { transform.DOScale(Vector3.one, duration / 2).SetEase(Ease.InQuad); });
            transform.DOMove(targetPosition, duration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    transform.DOShakePosition(0.3f, strength: 0.2f, vibrato: 10, randomness: 90, snapping: false,
                        fadeOut: true);
                    rotateTween.Kill();
                    Hide();
                    tsc.SetResult(true);
                });

            await tsc.Task;
        }
    }
}