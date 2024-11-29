using System.Collections.Generic;
using ArcaneWords.Scripts.Game.Gameplay.Level;
using ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons;
using ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.ButtonCreators;
using Zenject;

namespace ArcaneWords.Scripts.Game.Gameplay.Typewriter
{
    public class TypewriterModel
    {
        private const int LetterButtonIndex = -1;

        public ButtonsConfig ButtonsConfig { get; } = new();
        public LevelTimer LevelTimer => _levelTimer;
        public ButtonsTimer ButtonsTimer => _buttonsTimer;

        private Dictionary<int, ButtonCreator> _buttonCreators = new();
        private LevelTimer _levelTimer;
        private ButtonsTimer _buttonsTimer;

        [Inject]
        private void Construct(LevelTimer levelTimer, ButtonsTimer buttonsTimer)
        {
            _levelTimer = levelTimer;
            _buttonsTimer = buttonsTimer;
        }

        public void Init()
        {
            _levelTimer.Init();
            _levelTimer.StartLevelTimer();

            _buttonCreators[ButtonsConfig.ClearButtonPosition] = new ClearButtonCreator();
            _buttonCreators[ButtonsConfig.BackspaceButtonPosition] = new BackspaceButtonCreator();
            _buttonCreators[LetterButtonIndex] = new LetterButtonCreator();
        }

        public ButtonCreator GetButtonCreatorFromPosition(int position)
        {
            return _buttonCreators.TryGetValue(position, out var creator)
                ? creator
                : _buttonCreators[LetterButtonIndex];
        }

        public void Dispose()
        {
            _levelTimer.Dispose();
        }
    }
}