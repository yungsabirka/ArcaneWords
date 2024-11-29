using System.Collections;
using System.Linq;
using ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.Views;
using ArcaneWords.Scripts.Game.GameRoot;
using ArcaneWords.Scripts.Game.GameRoot.UI;
using DG.Tweening;
using ObservableCollections;
using TMPro;
using UnityEngine;
using R3;
using UnityEngine.UI;
using Zenject;

namespace ArcaneWords.Scripts.Game.Gameplay.Typewriter
{
    public class TypewriterView : MonoBehaviour
    {
        [SerializeField] private Transform _buttonsContainer;
        [SerializeField] private TextMeshProUGUI _writtenText;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private TextMeshProUGUI _guessedWordsCounter;
        [SerializeField] private Transform _bulbsViewContainer;

        private TypewriterViewModel _viewModel;
        private UIBulbsView _bulbsView;
        private GameAudioHandler _gameAudioHandler;
        private CompositeDisposable _disposables = new();

        [Inject]
        private void Construct(TypewriterViewModel viewModel, UIBulbsView bulbsView, GameAudioHandler gameAudioHandler)
        {
            _viewModel = viewModel;
            _bulbsView = bulbsView;
            _gameAudioHandler = gameAudioHandler;
        }

        public void Init(string levelName)
        {
            SetBulbsViewPosition();
            StartCoroutine(CreateButtons(levelName));
            SubscribeToButtonEvents();
            _disposables.Add(_viewModel.OnFoundingWord.Subscribe(ResetButtonsState));
            
            _disposables.Add(_viewModel.WrittenText
                .ObserveChanged()
                .Select(_ => string.Concat(_viewModel.WrittenText))
                .Subscribe(SetText));
            
            _disposables.Add(_viewModel.LevelTimer.LevelTime.Subscribe(SetLevelTime));
            
            _disposables.Add(_viewModel.GuessedWordsAmount.Subscribe(a =>
                SetGuessedWordsAmount(a, _viewModel.WordsAmount.CurrentValue)));
            
            _disposables.Add(_viewModel.WordsAmount.Subscribe(a =>
                SetGuessedWordsAmount(_viewModel.GuessedWordsAmount.CurrentValue, a)));
            
        }

        public void StartNewLevel(string levelName)
        {
            SetLettersToButtons(levelName);
        }
        
        private void SetLevelTime(int time)
        {
            var minutes = time / 60;
            var seconds = time - 60 * minutes;
            _timer.text = $"{minutes:00}:{seconds:00}";
        }

        private void SetGuessedWordsAmount(int guessedWordsAmount, int wordsAmount)
        {
            _guessedWordsCounter.text = $"{guessedWordsAmount} / {wordsAmount}";
        }

        private void SetText(string text)
        {
            _writtenText.text = text;
        }

        private void SetBulbsViewPosition()
        {
            _bulbsView.transform.SetParent(_bulbsViewContainer, false);
            var bulbsViewRect = _bulbsView.GetComponent<RectTransform>();

            bulbsViewRect.anchorMin = Vector2.zero;
            bulbsViewRect.anchorMax = Vector2.one;

            bulbsViewRect.offsetMin = Vector2.zero;
            bulbsViewRect.offsetMax = Vector2.zero;
        }

        private IEnumerator CreateButtons(string levelName)
        {
            for (int i = 0; i < _viewModel.ButtonsConfig.ButtonsAmount; i++)
            {
                var button = _viewModel
                    .GetButtonCreatorFromPosition(i)
                    .CreateButton();

                button.transform.SetParent(_buttonsContainer);
                button.transform.localScale = Vector3.one;
                _viewModel.AddButton(button);
                button.OnPressedSound.Subscribe(sound => _gameAudioHandler.PlaySound(sound));
            }

            yield return null;
            SetLettersToButtons(levelName);
        }

        private void SetLettersToButtons(string levelName)
        {
            var letters = levelName.ToList();

            for (int i = 0; i < _viewModel.ButtonsConfig.ButtonsAmount; i++)
            {
                if (_viewModel.Buttons[i] is not LetterButtonView letterButton)
                    continue;

                letterButton.SetLetter(' ');
                if (i < letters.Count)
                {
                    if (letterButton.IsActive == false)
                        letterButton.Activate();

                    letterButton.SetLetter(letters[i]);
                }
                else
                    letterButton.Deactivate();
            }
        }

        private void SubscribeToButtonEvents()
        {
            foreach (var button in _viewModel.Buttons)
                _disposables.Add(button.OnPressed.Subscribe(_ => _viewModel.OnButtonPressed(button)));
        }

        private void AnimateAndResetText()
        {
            _writtenText.color = Color.red;
            _writtenText.transform
                .DOShakePosition(2f, Vector3.right * 5f)
                .OnComplete(() =>
                {
                    _writtenText.color = Color.black;
                    _writtenText.text = string.Empty;
                    _viewModel.WrittenText.Clear();
                });
        }

        private void ResetButtonsState(bool isWordFound)
        {
            var activeLetterButtons = _viewModel.Buttons
                .Where(button => button.IsActive && button is LetterButtonView);

            if (isWordFound)
            {
                foreach (var button in activeLetterButtons)
                    if (button.IsPressed)
                        button.Release();
            }
            else
            {
                foreach (var button in activeLetterButtons)
                    StartCoroutine(ResetButton(button));
                AnimateAndResetText();
            }
        }

        private IEnumerator ResetButton(TypewriterButtonView buttonView)
        {
            buttonView.Deactivate();

            yield return new WaitForSeconds(buttonView.AnimateResetButtonTime);

            buttonView.Activate();
        }

        private void OnDestroy()
        {
            _viewModel.LevelTimer.SaveTimer();
            _disposables.Dispose();
            _viewModel.Dispose();
        }
    }
}