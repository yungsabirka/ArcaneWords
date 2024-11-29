using System.Collections.Generic;
using ArcaneWords.Scripts.Game.Gameplay.Root.Level;
using ArcaneWords.Scripts.Game.Gameplay.Typepaper;
using ArcaneWords.Scripts.Game.Gameplay.Typewriter;
using ArcaneWords.Scripts.Game.GameRoot.Level;
using ArcaneWords.Scripts.Game.GameRoot.UI;
using DG.Tweening;
using R3;
using Zenject;

namespace ArcaneWords.Scripts.Game.Gameplay.Level
{
    public class NextLevelStarter
    {
        private TypePaperView _typePaperView;
        private TypePaperModel _typePaperModel;
        private TypePaperViewModel _typePaperViewModel;
        private TypewriterView _typewriterView;
        private TypewriterViewModel _typewriterViewModel;
        private TypewriterModel _typewriterModel;
        private UITitleView _titleView;
        private LevelsHandler _levelsHandler;
        
        private CompositeDisposable _disposables = new();
        private int _currentNotifierReadyCount;
        
        [Inject]
        public void Construct(TypePaperView typePaperView, TypePaperModel typePaperModel, TypePaperViewModel typePaperViewModel,
            TypewriterModel typewriterModel, TypewriterView typewriterView, TypewriterViewModel typewriterViewModel,
            UITitleView titleView, LevelsHandler levelsHandler)
        {
            _typePaperView = typePaperView;
            _typePaperModel = typePaperModel;
            _typePaperViewModel = typePaperViewModel;
            _typewriterView = typewriterView;
            _typewriterViewModel = typewriterViewModel;
            _typewriterModel = typewriterModel;
            _titleView = titleView;
            _levelsHandler = levelsHandler;
        }

        public void Init()
        {
            SetNotifiers();
        }
        
        private void SetNotifiers()
        {
            var notifiers = new List<ILevelReadyNotifier>
            {
                _typePaperModel,
                _typePaperViewModel,
            };
            foreach (var notifier in notifiers)
            {
                _disposables.Add(
                    notifier.OnReadyForLevelTransition.Subscribe(u => HandleReadyForLevelTransition(notifiers.Count)));
            }
        }

        private void HandleReadyForLevelTransition(int totalNotifiersAmount)
        {
            _currentNotifierReadyCount++;
            if (_currentNotifierReadyCount != totalNotifiersAmount)
                return;

            _currentNotifierReadyCount = 0;
            StartNewLevel();
        }

        private void StartNewLevel()
        {
            _levelsHandler.SaveCurrentLevel();
            var levelInfo = _levelsHandler.SetAndReturnNextLevelInfo();
            var levelName = _levelsHandler.GetCurrentLevelName();

            var typePaperPosition = _typePaperView.transform.position;
            var typePaperRotation = _typePaperView.transform.rotation;
            _typePaperView.HidePaper()
                .OnComplete(() =>
                {
                    _typePaperViewModel.DestroyAllWords();
                    _typePaperModel.Init();
                    _typewriterView.StartNewLevel(levelName);
                    _titleView.SetTitle(levelName);
                    _typewriterModel.Init();
                    _typewriterViewModel.Init();
                    _typePaperView.ShowPaper(typePaperPosition, typePaperRotation);
                });
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}