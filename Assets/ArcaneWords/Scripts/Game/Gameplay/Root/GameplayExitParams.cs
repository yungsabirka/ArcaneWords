using ArcaneWords.Scripts.Game.MainMenu.Root;

namespace ArcaneWords.Scripts.Game.Gameplay.Root
{
    public class GameplayExitParams
    {
        public MainMenuEnterParams MainMenuEnterParams { get; }

        public GameplayExitParams(MainMenuEnterParams mainMenuEnterParams)
        {
            MainMenuEnterParams = mainMenuEnterParams;
        }
    }
}