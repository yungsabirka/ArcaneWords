using System;
using ArcaneWords.Scripts.Game.GameRoot.Bank;
using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace ArcaneWords.Scripts.Game.GameRoot.UI
{
    public class UICoinsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinsText;
        
        private BankViewModel _bankViewModel;
        private IDisposable _subscription;

        [Inject]
        private void Construct(BankViewModel bankViewModel)
        {
            _bankViewModel = bankViewModel;
        }

        public void Init()
        {
            _subscription = _bankViewModel.Coins.Subscribe(SetCoins);
        }

        private void SetCoins(int coins)
        {
            if (int.TryParse(_coinsText.text, out var currentCoins) == false)
            {
                _coinsText.text = coins.ToString();
                return;
            }

            var targetColor = currentCoins < coins ? Color.green : Color.red;

            _coinsText.DOColor(targetColor, 0.2f);

            DOTween.To(() => currentCoins, x =>
                {
                    currentCoins = x;
                    _coinsText.text = currentCoins.ToString();
                }, coins, 2f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    _coinsText.DOColor(Color.black, 0.2f);
                });
        }
        
        private void OnDestroy()
        {
            _subscription?.Dispose();
        }
    }
}