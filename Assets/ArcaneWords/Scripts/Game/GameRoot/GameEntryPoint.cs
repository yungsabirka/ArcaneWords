using System.Collections;
using ArcaneWords.Scripts.Game.Gameplay.Root;
using ArcaneWords.Scripts.Game.GameRoot.Level;
using ArcaneWords.Scripts.Game.GameRoot.UI;
using ArcaneWords.Scripts.Game.MainMenu.Root;
using UnityEngine;
using UnityEngine.SceneManagement;
using R3;
using Zenject;

namespace ArcaneWords.Scripts.Game.GameRoot
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;
        private Coroutines _coroutines;
        private UIRootView _uiRoot;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            _instance = new GameEntryPoint();
            _instance.RunGame();
        }

        private GameEntryPoint()
        {
            _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
            var gameAudioHandler = new GameObject("AudioHandler")
                .AddComponent<AudioSource>().gameObject
                .AddComponent<GameAudioHandler>();
            var prefabUIRoot = Resources.Load<UIRootView>("Prefabs/UI/UIRoot");
            var prefabLevelsHandler = Resources.Load<LevelsHandler>("Prefabs/Levels/LevelsHandler");

            _uiRoot = Object.Instantiate(prefabUIRoot);
            var levelsHandler = Object.Instantiate(prefabLevelsHandler);

            Object.DontDestroyOnLoad(_coroutines.gameObject);
            Object.DontDestroyOnLoad(_uiRoot.gameObject);
            Object.DontDestroyOnLoad(levelsHandler.gameObject);
            Object.DontDestroyOnLoad(gameAudioHandler.gameObject);

            var container = ProjectContext.Instance.Container;
            var gameInstaller = new GameInstaller(_uiRoot, _coroutines, levelsHandler, gameAudioHandler);
            container.Inject(gameInstaller);
            gameInstaller.InstallBindings();
        }

        private void RunGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            switch (sceneName)
            {
                case SceneNames.Gameplay:
                {
                    var enterParams = new GameplayEnterParams("Angina");
                    _coroutines.StartCoroutine(LoadAndStartGameplayAsync(enterParams));
                    break;
                }
                case SceneNames.MainMenu:
                    _coroutines.StartCoroutine(LoadAndStartMainMenuAsync());
                    break;
            }

            if (sceneName != SceneNames.MainMenu)
                return;
#endif
            _coroutines.StartCoroutine(LoadAndStartMainMenuAsync());
        }

        private IEnumerator LoadAndStartGameplayAsync(GameplayEnterParams enterParams = null)
        {
            _uiRoot.ShowLoadingScreen();

            yield return LoadSceneAsync(SceneNames.Boot);
            yield return null;
            yield return LoadSceneAsync(SceneNames.Gameplay);
            yield return null;

            _uiRoot.ClearSceneUI();
            var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
            sceneEntryPoint.Run(enterParams).Subscribe(gameplayExitParams =>
            {
                _coroutines.StartCoroutine(
                    LoadAndStartMainMenuAsync(gameplayExitParams.MainMenuEnterParams));
            });
            _uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadAndStartMainMenuAsync(MainMenuEnterParams enterParams = null)
        {
            _uiRoot.ShowLoadingScreen();

            yield return LoadSceneAsync(SceneNames.Boot);
            yield return null;
            yield return LoadSceneAsync(SceneNames.MainMenu);
            yield return null;

            _uiRoot.ClearSceneUI();
            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            sceneEntryPoint.Run(enterParams).Subscribe(mainMenuExitParams =>
            {
                var targetSceneName = mainMenuExitParams.TargetSceneEnterParams.SceneName;
                if (targetSceneName == SceneNames.Gameplay)
                {
                    _coroutines.StartCoroutine(
                        LoadAndStartGameplayAsync(mainMenuExitParams.TargetSceneEnterParams.As<GameplayEnterParams>()));
                }
            });

            _uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}