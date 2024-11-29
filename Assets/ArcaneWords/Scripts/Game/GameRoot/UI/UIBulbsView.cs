using System;
using ArcaneWords.Scripts.Game.GameRoot.Bank;
using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace ArcaneWords.Scripts.Game.GameRoot.UI
{
    public class UIBulbsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _bulbsText;

        private BankViewModel _bankViewModel;
        private CompositeDisposable _subscriptions = new();
        private bool _isAnimating = false;

        [Inject]
        private void Construct(BankViewModel bankViewModel)
        {
            _bankViewModel = bankViewModel;
        }

        public void Init()
        {
            _subscriptions.Add(_bankViewModel.Bulbs.Subscribe(SetBulbs));
            _subscriptions.Add(_bankViewModel.OnSpendBulbsFailed.Subscribe(e => AnimateSpendFailure()));
        }

        private void AnimateSpendFailure()
        {
            if (_isAnimating) return;

            _isAnimating = true;

            var originalColor = _bulbsText.color;
            var originalScale = _bulbsText.transform.localScale;

            _bulbsText.DOColor(Color.red, 0.2f);
            _bulbsText.transform.DOScale(originalScale * 1.2f, 0.2f)
                .OnComplete(() =>
                {
                    _bulbsText.DOColor(originalColor, 0.2f);
                    _bulbsText.transform.DOScale(originalScale, 0.2f)
                        .OnComplete(() => _isAnimating = false);
                });
        }

        private void SetBulbs(int bulbs)
        {
            _bulbsText.text = bulbs.ToString();
        }

        private void OnDestroy()
        {
            _subscriptions?.Dispose();
        }
    }
}