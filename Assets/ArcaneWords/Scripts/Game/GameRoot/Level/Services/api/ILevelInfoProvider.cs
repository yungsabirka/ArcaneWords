namespace ArcaneWords.Scripts.Game.GameRoot.Level.Services.api
{
    public interface ILevelInfoProvider
    {
        LevelInfo LoadLevelInfo(string levelName);
        void SaveLevelInfo(string levelName, LevelInfo levelInfo);
    }
}