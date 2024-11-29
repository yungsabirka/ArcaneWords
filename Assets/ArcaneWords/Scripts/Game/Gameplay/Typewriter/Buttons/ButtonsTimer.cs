using R3;
using UnityEngine;
using Zenject;

namespace ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons
{
    public class ButtonsTimer : ITickable
    {
        public Subject<Unit> OnTimerComplete { get; } = new();
        
        private float _timeFromButtonPressed;
        private float _maxTimeFromButtonPressed = 2f;
        private bool _isButtonsTimerRunning;
        
        public void Tick()
        {
            if (_isButtonsTimerRunning == false) return;

            _timeFromButtonPressed += Time.deltaTime;

            if (_timeFromButtonPressed >= _maxTimeFromButtonPressed)
            {
                Stop();
                Reset();
                OnTimerComplete.OnNext(Unit.Default);
            }
        }

        
        public void Start()
        {
            _isButtonsTimerRunning = true;
        }

        public void Stop()
        {
            _isButtonsTimerRunning = false;
        }

        public void Reset()
        {
            _timeFromButtonPressed = 0;
        }
    }
}