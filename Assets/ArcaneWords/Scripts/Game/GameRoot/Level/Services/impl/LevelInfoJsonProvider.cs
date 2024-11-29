using System.IO;
using ArcaneWords.Scripts.Game.GameRoot.Level.Services.api;
using UnityEngine;

namespace ArcaneWords.Scripts.Game.GameRoot.Level.Services.impl
{
    public class LevelInfoJsonProvider : ILevelInfoProvider
    {
        public LevelInfo LoadLevelInfo(string levelName)
        {
            var levelInfoPath = Path.Combine(Application.persistentDataPath, levelName);
            if (File.Exists(levelInfoPath) == false)
                return null;

            var jsonContent = File.ReadAllText(levelInfoPath);
            return jsonContent == string.Empty ? null : JsonUtility.FromJson<LevelInfo>(jsonContent);
        }

        public void SaveLevelInfo(string levelName, LevelInfo levelInfo)
        {
            var levelInfoPath = Path.Combine(Application.persistentDataPath, levelName);
            var levelInfoJson = JsonUtility.ToJson(levelInfo);
            File.WriteAllText(levelInfoPath, levelInfoJson);
        }
    }
}