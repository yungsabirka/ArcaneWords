using System;
using System.Linq;
using System.Threading.Tasks;
using ArcaneWords.Scripts.Game.Gameplay.Popups;
using ArcaneWords.Scripts.Game.Gameplay.Typepaper.Word;
using DG.Tweening;
using ModestTree;
using UnityEngine;
using Zenject;
using R3;
using UnityEngine.UI;

namespace ArcaneWords.Scripts.Game.Gameplay.Typepaper
{
    public class TypePaperView : MonoBehaviour
    {
        [SerializeField] private WordView _wordViewPrefab;
        [SerializeField] private Transform _wordsContainer;
        [SerializeField] private ScrollRect _scrollRect;

        private TypePaperViewModel _viewModel;
        private UIHintPopupView _uiHintPopupView;
        private UICoinPopupView _uiCoinPopupView;
        private CompositeDisposable _disposables = new();

        [Inject]
        private void Construct(TypePaperViewModel viewModel, UIHintPopupView uiHintPopupView, UICoinPopupView uiCoinPopupView)
        {
            _viewModel = viewModel;
            _uiHintPopupView = uiHintPopupView;
            _uiCoinPopupView = uiCoinPopupView;
        }

        public void Init()
        {
            _disposables.Add(_viewModel.OnWordViewModelCreated.Subscribe(CreateWordView));
            _disposables.Add(_viewModel.OnOpeningWord.Subscribe(OpenWord));
            _disposables.Add(_viewModel.OnOpeningWordAgain.Subscribe(OpenWordAgain));
            ShowPaper(transform.position, transform.rotation);
        }

        public Tween ShowPaper(Vector3 endPosition, Quaternion endRotation)
        {
            var offScreenPosition = new Vector3(-Screen.width, transform.position.y + 100f, transform.position.z);
            transform.position = offScreenPosition;

            var movingRotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 5f);
            transform.rotation = movingRotation;


            var sequence = DOTween.Sequence();

            sequence
                .Append(transform.DOMoveX(endPosition.x, 1f).SetEase(Ease.InOutCubic))
                .Append(transform.DORotateQuaternion(endRotation, 0.2f).SetEase(Ease.InOutQuad))
                .Join(transform.DOMoveY(endPosition.y, 0.2f).SetEase(Ease.InQuad));

            return sequence.Play();
        }

        public Tween HidePaper()
        {
            var offScreenPosition = new Vector3(2 * Screen.width, transform.position.y + 100f, transform.position.z);
            var sequence = DOTween.Sequence();

            sequence
                .Append(transform.DOMoveY(transform.position.y + 50f, 0.5f).SetEase(Ease.OutQuad))
                .Append(transform.DOMove(offScreenPosition, 0.5f).SetEase(Ease.InQuad));

            return sequence.Play();
        }

        private void CreateWordView(WordViewModel wordViewModel)
        {
            var wordView = Instantiate(_wordViewPrefab, _wordsContainer);
            wordView.Init(wordViewModel, _uiHintPopupView);
            _viewModel.RegisterWordView(wordView, wordViewModel);
        }

        private async void OpenWord(WordView wordView)
        {
            await FocusOnFoundWord(wordView);
            wordView.OpenWord();
            _uiCoinPopupView.Show();
            await _uiCoinPopupView.MoveToCoinsView(wordView.transform.position);
            _viewModel.AddCoins(wordView);
        }

        private async void OpenWordAgain(WordView wordView)
        {
            await FocusOnFoundWord(wordView);
            wordView.OpenWordAgain();
        }

        private async Task FocusOnFoundWord(WordView wordView)
        {
            var scrollTransform = _scrollRect.transform as RectTransform;
            var content = _scrollRect.content;

            var targetPosition = scrollTransform.InverseTransformPoint(wordView.transform.position);
            var centerPosition = scrollTransform.InverseTransformPoint(scrollTransform.position);
            var difference = centerPosition - targetPosition;

            if (_scrollRect.horizontal == false) difference.x = 0f;
            if (_scrollRect.vertical == false) difference.y = 0f;

            var normalizedDifference = new Vector2(
                difference.x / (content.rect.width - scrollTransform.rect.width),
                difference.y / (content.rect.height - scrollTransform.rect.height));

            var newNormalizedPosition = _scrollRect.normalizedPosition - normalizedDifference;

            if (_scrollRect.movementType != ScrollRect.MovementType.Unrestricted)
            {
                newNormalizedPosition = new Vector2(
                    Mathf.Clamp01(newNormalizedPosition.x),
                    Mathf.Clamp01(newNormalizedPosition.y));
            }

            var tsc = new TaskCompletionSource<bool>();

            DOTween.To(
                    () => _scrollRect.normalizedPosition,
                    value => _scrollRect.normalizedPosition = value,
                    newNormalizedPosition,
                    _viewModel.FindWordDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => tsc.SetResult(true));

            await tsc.Task;
        }

        private void OnDestroy()
        {
            _viewModel.Dispose();
            _disposables.Dispose();
        }
    }
}