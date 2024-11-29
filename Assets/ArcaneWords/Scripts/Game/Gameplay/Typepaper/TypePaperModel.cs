using System;
using System.Collections.Generic;
using System.Linq;
using ArcaneWords.Scripts.Game.Gameplay.Popups;
using ArcaneWords.Scripts.Game.Gameplay.Root.Level;
using ArcaneWords.Scripts.Game.Gameplay.Typepaper.Word;
using ArcaneWords.Scripts.Game.GameRoot.Level;
using R3;
using Zenject;

namespace ArcaneWords.Scripts.Game.Gameplay.Typepaper
{
    public class TypePaperModel : ILevelReadyNotifier
    {
        public Subject<Unit> OnReadyForLevelTransition { get; } = new();
        public Subject<Unit> OnWordsSet = new();

        public IReadOnlyList<WordModel> Words => _words;
        public ReadOnlyReactiveProperty<int> WordsAmount => _wordsAmount;
        public ReadOnlyReactiveProperty<int> GuessedWordsAmount => _guessedWordsAmount;
        public Subject<string> OnWordFound { get; } = new();
        public bool IsReadyForLevelTransition { get; private set; }

        private List<WordModel> _words = new();
        private ReactiveProperty<int> _wordsAmount = new();
        private ReactiveProperty<int> _guessedWordsAmount = new();
        private LevelsHandler _levelHandler;
        private UIHintPopupView _uiHintPopupView;
        private CompositeDisposable _disposable = new();

        [Inject]
        private void Construct(LevelsHandler levelHandler, UIHintPopupView uiHintPopupView)
        {
            _levelHandler = levelHandler;
            _uiHintPopupView = uiHintPopupView;
        }

        public void Init()
        {
            if (_disposable.Count > 0)
                _disposable.Dispose();

            _disposable.Add(_uiHintPopupView.OnDescriptionBought.Subscribe(AddBoughtDescription));
            IsReadyForLevelTransition = false;
            _words = _levelHandler.LoadCurrentLevelWords();
            _wordsAmount.Value = _words.Count;

            var levelInfo = _levelHandler.GetCurrentLevelInfo();
            _guessedWordsAmount.Value = levelInfo.GuessedWords.Count;
            OnWordsSet.OnNext(Unit.Default);
        }

        public bool TryToFindWord(string word)
        {
            var matchedWord = _words.Find(
                w => string.Equals(w.Word, word, StringComparison.OrdinalIgnoreCase));

            if (matchedWord == null)
                return false;

            var levelInfo = _levelHandler.GetCurrentLevelInfo();

            if (GetGuessedWordState(word) == false)
            {
                levelInfo.GuessedWords.Add(matchedWord);
                _guessedWordsAmount.Value++;
                if (_guessedWordsAmount.Value == _words.Count)
                {
                    OnReadyForLevelTransition?.OnNext(Unit.Default);
                    IsReadyForLevelTransition = true;
                }
            }

            OnWordFound.OnNext(matchedWord.Word);
            return true;
        }

        private void AddBoughtDescription(string word)
        {
            var levelInfo = _levelHandler.GetCurrentLevelInfo();
            var wordModel = _words.Find(w => string.Equals(w.Word, word, StringComparison.OrdinalIgnoreCase));
            levelInfo.WordsWithHints.Add(wordModel);
        }

        public bool GetDescriptionWordState(string word)
        {
            var levelInfo = _levelHandler.GetCurrentLevelInfo();
            return levelInfo.WordsWithHints.Any(w => string.Equals(w.Word, word, StringComparison.OrdinalIgnoreCase));
        }

        public bool GetGuessedWordState(string word)
        {
            var levelInfo = _levelHandler.GetCurrentLevelInfo();
            return levelInfo.GuessedWords.Any(w => string.Equals(w.Word, word, StringComparison.OrdinalIgnoreCase));
        }
    }
}