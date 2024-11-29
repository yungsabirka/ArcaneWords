using ArcaneWords.Scripts.Game.GameRoot.Level;
using ArcaneWords.Scripts.Game.GameRoot.UI;
using Zenject;

namespace ArcaneWords.Scripts.Game.GameRoot
{
    public class GameInstaller : Installer
    {
        private UIRootView _uiRootView;
        private Coroutines _coroutines;
        private LevelsHandler _levelsHandler;
        private GameAudioHandler _gameAudioHandler;

        public GameInstaller(UIRootView uiRootView, Coroutines coroutines, LevelsHandler levelsHandler, GameAudioHandler gameAudioHandler)
        {
            _uiRootView = uiRootView;
            _coroutines = coroutines;
            _levelsHandler = levelsHandler;
            _gameAudioHandler = gameAudioHandler;
        }
        
        public override void InstallBindings()
        {
            Container.Bind<UIRootView>().FromInstance(_uiRootView).AsSingle();
            Container.Bind<Coroutines>().FromInstance(_coroutines).AsSingle();
            Container.Bind<LevelsHandler>().FromInstance(_levelsHandler).AsSingle();
            Container.Bind<GameAudioHandler>().FromInstance(_gameAudioHandler).AsSingle();
        }
    }
}