using System;
using System.Collections.Generic;
using System.Linq;
using ArcaneWords.Scripts.Game.Gameplay.Typepaper.Word;
using ArcaneWords.Scripts.Game.GameRoot.Level.Services.api;
using ArcaneWords.Scripts.Game.GameRoot.Level.Services.impl;
using ArcaneWords.Scripts.Utils;
using UnityEngine;

namespace ArcaneWords.Scripts.Game.GameRoot.Level
{
    public class LevelsHandler : MonoBehaviour
    {
        [SerializeField] private List<TextAsset> _jsonLevels;

        private Dictionary<TextAsset, LevelInfo> _levelsInfoMap = new();
        private ILevelInfoProvider _levelInfoProvider;
        private LevelInfo _currentLevelInfo;

        public void Start()
        {
            _levelInfoProvider = new LevelInfoJsonProvider();
            foreach (var levelText in _jsonLevels)
            {
                var levelInfo = _levelInfoProvider.LoadLevelInfo(levelText.name);
                if (levelInfo == null)
                {
                    levelInfo = new LevelInfo();
                    _levelInfoProvider.SaveLevelInfo(levelText.name, levelInfo);
                }
                _levelsInfoMap.Add(levelText, levelInfo);
            }
        }

        public void SetCurrentLevel(string levelName)
        {
            var levelText = _jsonLevels.First(x => x.name == levelName);
            if(levelText == null)
                throw new ArgumentException($"Can not set level with name {levelName}");

            _currentLevelInfo = _levelsInfoMap[levelText];
        }

        public LevelInfo GetCurrentLevelInfo()
        {
            return _currentLevelInfo;
        }

        public string GetCurrentLevelName()
        {
            return GetCurrentLevelTextAsset().name;
        }

        public List<string> GetLevelNames()
        {
            return _jsonLevels.Select(v => v.name).ToList();
        }

        public LevelInfo GetLevelInfoByName(string name)
        {
            return _levelsInfoMap.First(x => x.Key.name == name).Value;
        }

        public int GetLevelWordsAmountByName(string name)
        {
            var levelText = _levelsInfoMap.First(v => v.Key.name == name).Key;
            return JsonUtilityListHelper.ListFromJson<WordModel>(levelText.text).Count;
        }

        public LevelInfo SetAndReturnNextLevelInfo()
        {
            SaveCurrentLevel();
            var levelText = GetCurrentLevelTextAsset();
            var currentIndex = _jsonLevels.IndexOf(levelText);

            if (currentIndex + 1 >= _jsonLevels.Count)
                return null;
            
            var nextLevelText = _jsonLevels[currentIndex + 1];
            _currentLevelInfo = _levelsInfoMap[nextLevelText];
            return _currentLevelInfo;
        }

        public List<WordModel> LoadCurrentLevelWords()
        {
            var levelText = GetCurrentLevelTextAsset();
            return JsonUtilityListHelper.ListFromJson<WordModel>(levelText.text);
        }
        
        public void SaveCurrentLevel()
        {
            var levelName = GetCurrentLevelTextAsset().name;
            _levelInfoProvider.SaveLevelInfo(levelName, _currentLevelInfo);
        }
        
        private TextAsset GetCurrentLevelTextAsset()
        {
            return _levelsInfoMap.First(v => v.Value == _currentLevelInfo).Key;
        }
    }
}