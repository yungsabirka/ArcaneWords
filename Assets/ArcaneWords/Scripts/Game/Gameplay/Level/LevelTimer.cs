using System.Collections;
using ArcaneWords.Scripts.Game.Gameplay.Popups;
using ArcaneWords.Scripts.Game.GameRoot.Level;
using R3;
using UnityEngine;
using Zenject;

namespace ArcaneWords.Scripts.Game.Gameplay.Level
{
    public class LevelTimer
    {
        public ReadOnlyReactiveProperty<int> LevelTime => _levelTime;
        
        private ReactiveProperty<int> _levelTime = new();
        private bool _isLevelTimerRunning;
        private LevelsHandler _levelsHandler;
        private UIHintPopupView _uiHintPopupView;
        private Coroutines _coroutines;
        private CompositeDisposable _disposable = new();

        [Inject]
        private void Construct(LevelsHandler levelsHandler, UIHintPopupView uiHintPopupView, Coroutines coroutines)
        {
            _levelsHandler = levelsHandler;
            _uiHintPopupView = uiHintPopupView;
            _coroutines = coroutines;
        }

        public void Init()
        {
            if(_disposable.Count > 0)
                _disposable.Dispose();
            
            _disposable.Add(_uiHintPopupView.OnPopupShown.Subscribe(_ => StopLevelTimer()));
            _disposable.Add(_uiHintPopupView.OnPopupHidden.Subscribe(_ => StartLevelTimer()));

            var levelInfo = _levelsHandler.GetCurrentLevelInfo();
            SetTime(levelInfo.Time);
        }

        public void StartLevelTimer()
        {
            _coroutines.StartCoroutine(LevelTimerRoutine());
        }
        
        private IEnumerator LevelTimerRoutine()
        {
            if (_isLevelTimerRunning)
                yield break;

            _isLevelTimerRunning = true;
            while (_isLevelTimerRunning)
            {
                _levelTime.Value++;
                yield return new WaitForSeconds(1f);
            }
        }
        
        public void SaveTimer()
        {
            var levelInfo = _levelsHandler.GetCurrentLevelInfo();
            levelInfo.Time = _levelTime.Value;
        }
        
        private void SetTime(int time)
        {
            _levelTime.Value = time;
        }
        
        private void StopLevelTimer()
        {
            _isLevelTimerRunning = false;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}