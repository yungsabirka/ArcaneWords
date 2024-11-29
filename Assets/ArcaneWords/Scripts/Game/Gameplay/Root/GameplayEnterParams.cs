using ArcaneWords.Scripts.Game.GameRoot;

namespace ArcaneWords.Scripts.Game.Gameplay.Root
{
    public class GameplayEnterParams : SceneEnterParams
    {
        public readonly string LevelName;
        
        public GameplayEnterParams(string levelName) : base(SceneNames.Gameplay)
        {
            LevelName = levelName;
        }
    }
}
