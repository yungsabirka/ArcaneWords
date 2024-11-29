using System.Collections.Generic;
using System.Linq;
using ArcaneWords.Scripts.Game.Gameplay.Root.Level;
using ArcaneWords.Scripts.Game.Gameplay.Typepaper.Word;
using ArcaneWords.Scripts.Game.GameRoot.Bank;
using R3;
using UnityEngine;
using Zenject;

namespace ArcaneWords.Scripts.Game.Gameplay.Typepaper
{
    public class TypePaperViewModel : ILevelReadyNotifier
    {
        public Subject<Unit> OnReadyForLevelTransition { get; } = new();
        public Subject<WordViewModel> OnWordViewModelCreated = new();
        public Subject<WordView> OnOpeningWord = new();
        public Subject<WordView> OnOpeningWordAgain = new();
        
        public readonly float FindWordDuration = 0.3f;
        
        private readonly Dictionary<WordViewModel, WordView> _wordViewModelsMap = new();
        private TypePaperModel _model;
        private BankViewModel _bankViewModel;
        private CompositeDisposable _disposable = new();
        
        [Inject]
        private void Construct(TypePaperModel model, BankViewModel bankViewModel)
        {
            _model = model;
            _bankViewModel = bankViewModel;
        }

        public void Init()
        {
            _disposable.Add(_model.OnWordsSet.Subscribe(_ => CreateWordViewModels()));
            _disposable.Add(_model.OnWordFound.Subscribe(FindWordViewModel));
        }

        public void DestroyAllWords()
        {
            foreach (var pair in _wordViewModelsMap)
                Object.Destroy(pair.Value.gameObject);
            
            _wordViewModelsMap.Clear();
        }

        private void CreateWordViewModels()
        {
            foreach (var word in _model.Words)
            {
                var wordViewModel = new WordViewModel(word);
                if (_model.GetDescriptionWordState(word.Word))
                    wordViewModel.AllowDescription();   
                
                if(_model.GetGuessedWordState(word.Word))
                    wordViewModel.OpenWord();
                
                OnWordViewModelCreated.OnNext(wordViewModel);
            }   
        }

        public void RegisterWordView(WordView wordView, WordViewModel wordViewModel)
        {
            _wordViewModelsMap[wordViewModel] = wordView;
        }

        public void AddCoins(WordView wordView)
        {
            var wordViewModel = _wordViewModelsMap.First(pair => pair.Value == wordView).Key;
            var coins = wordViewModel.Word.Length * 10;
            _bankViewModel.AddCoins(coins);
            if (_model.IsReadyForLevelTransition)
            {
                OnReadyForLevelTransition.OnNext(Unit.Default);   
            }
        }

        private void FindWordViewModel(string word)
        {
            var (wordViewModel, wordView) = _wordViewModelsMap.First(vm => vm.Key.Word == word);

            if (wordViewModel.IsWordAvailable.CurrentValue)
            {
                OnOpeningWordAgain.OnNext(wordView);
            }
            else
            {
                wordViewModel.OpenWord();
                OnOpeningWord.OnNext(wordView);
            }
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}