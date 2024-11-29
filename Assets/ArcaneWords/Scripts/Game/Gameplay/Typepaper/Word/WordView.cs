using System;
using ArcaneWords.Scripts.Game.Gameplay.Popups;
using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneWords.Scripts.Game.Gameplay.Typepaper.Word
{
    public class WordView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _wordText;
        [SerializeField] private Button _button;

        private WordViewModel _viewModel;
        private CompositeDisposable _disposables = new();

        public void Init(WordViewModel viewModel, UIHintPopupView uiHintPopupView)
        {
            _viewModel = viewModel;
            if (viewModel.IsWordAvailable.CurrentValue)
            {
                SetWord(_viewModel.Word);
            }
            else
            {
                var closedWord = new string('-', _viewModel.Word.Length);
                SetWord(closedWord);

                _disposables.Add(
                    _viewModel.IsWordAvailable.Subscribe(val =>
                    {
                        if (val)
                            SetWord(_viewModel.Word);
                    }));   
            }

            _button.onClick.AddListener(() => OnWordClicked(uiHintPopupView));
        }

        private void SetWord(string word)
        {
            _wordText.text = word;
        }

        private void OnWordClicked(UIHintPopupView uiHintPopupView)
        {
            uiHintPopupView.SetWord(_viewModel);
            uiHintPopupView.Show();
        }

        private void AnimateGuessedWord(Color color)
        {
            _wordText.color = color;
            var sequence = DOTween.Sequence();

            sequence.Append(transform.DOScale(1.5f, .5f));
            sequence.Append(transform.DOScale(1f, .5f));
            sequence.OnComplete(() => _wordText.color = Color.black);
        }

        public void OpenWord()
        {
            AnimateGuessedWord(Color.green);
        }

        public void OpenWordAgain()
        {
            AnimateGuessedWord(Color.red);
        }

        private void Dispose()
        {
            _button.onClick.RemoveAllListeners();
            _disposables?.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}