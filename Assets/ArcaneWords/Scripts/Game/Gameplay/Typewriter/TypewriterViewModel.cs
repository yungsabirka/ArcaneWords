using System.Collections.Generic;
using System.Linq;
using ArcaneWords.Scripts.Game.Gameplay.Level;
using ArcaneWords.Scripts.Game.Gameplay.Typepaper;
using ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons;
using ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.ButtonCreators;
using ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.Views;
using ObservableCollections;
using R3;
using Zenject;

namespace ArcaneWords.Scripts.Game.Gameplay.Typewriter
{
    public class TypewriterViewModel
    {
        public Subject<bool> OnFoundingWord = new();
        public ObservableList<char> WrittenText { get; } = new();
        public ButtonsConfig ButtonsConfig => _typewriterModel.ButtonsConfig;
        public IReadOnlyList<TypewriterButtonView> Buttons => _buttons;
        public ReadOnlyReactiveProperty<int> WordsAmount => _wordsAmount;
        public ReadOnlyReactiveProperty<int> GuessedWordsAmount => _guessedWordsAmount;
        public LevelTimer LevelTimer => _typewriterModel.LevelTimer;

        private List<TypewriterButtonView> _buttons = new();
        private ReactiveProperty<int> _wordsAmount = new();
        private ReactiveProperty<int> _guessedWordsAmount = new();

        private TypePaperModel _typePaperModel;
        private TypewriterModel _typewriterModel;
        private CompositeDisposable _disposables = new();

        [Inject]
        private void Construct(TypewriterModel typewriterModel, TypePaperModel typePaperModel)
        {
            _typePaperModel = typePaperModel;
            _typewriterModel = typewriterModel;
        }

        public void Init()
        {
            if (_disposables.Count > 0)
                _disposables.Dispose();

            _disposables.Add(
                _typePaperModel.WordsAmount.Subscribe(amount => _wordsAmount.Value = amount));
            _disposables.Add(
                _typePaperModel.GuessedWordsAmount.Subscribe(amount => _guessedWordsAmount.Value = amount));
            _disposables.Add(
                _typewriterModel.ButtonsTimer.OnTimerComplete.Subscribe(_ => HandleTimerComplete()));
        }

        public ButtonCreator GetButtonCreatorFromPosition(int position)
        {
            return _typewriterModel.GetButtonCreatorFromPosition(position);
        }

        public void AddButton(TypewriterButtonView buttonView)
        {
            _buttons.Add(buttonView);
        }

        public void OnButtonPressed(TypewriterButtonView buttonView)
        {
            ProcessButtonPress(buttonView);

            if (WrittenText.Count > 0)
                _typewriterModel.ButtonsTimer.Start();
            else
                _typewriterModel.ButtonsTimer.Stop();
        }

        private void ProcessButtonPress(TypewriterButtonView buttonView)
        {
            switch (buttonView)
            {
                case ClearButtonView:
                    foreach (var b in _buttons)
                        if (b is LetterButtonView letterButton && b.IsActive && b.IsPressed)
                            letterButton.Release();
                    break;

                case BackspaceButtonView when WrittenText.Count > 0:
                    var lastPressedButton = _buttons
                        .OfType<LetterButtonView>()
                        .FirstOrDefault(b => b.Letter == WrittenText[^1] && b.IsPressed);

                    lastPressedButton?.Release();
                    break;
            }

            buttonView.InteractWithText(WrittenText);
            _typewriterModel.ButtonsTimer.Reset();
        }

        private void HandleTimerComplete()
        {
            var isFound = _typePaperModel.TryToFindWord(string.Concat(WrittenText));
            OnFoundingWord.OnNext(isFound);
            if (isFound)
                WrittenText.Clear();
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _typewriterModel.Dispose();
        }
    }
}