using ArcaneWords.Scripts.Game.Gameplay.Root;
using R3;
using UnityEngine;

namespace ArcaneWords.Scripts.Game.MainMenu.Root.View
{
    public class UIMainMenuRootBinder : MonoBehaviour
    {
        private Subject<MainMenuExitParams> _exitSceneSignalSubject;
        
        public void HandleGoToGameplayButtonClick(string levelName)
        {
            var gameplayEnterParams = new GameplayEnterParams(levelName);
            var mainMenuExitParams = new MainMenuExitParams(gameplayEnterParams);
            _exitSceneSignalSubject?.OnNext(mainMenuExitParams);
        }

        public void Bind(Subject<MainMenuExitParams> exitSceneSignalSubject)
        {
            _exitSceneSignalSubject = exitSceneSignalSubject;
        }
    }
}