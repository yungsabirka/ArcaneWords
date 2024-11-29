using System;
using System.Collections.Generic;
using ArcaneWords.Scripts.Game.Gameplay.Typepaper.Word;

namespace ArcaneWords.Scripts.Game.GameRoot.Level
{
    [Serializable]
    public class LevelInfo
    {
        public List<WordModel> GuessedWords = new();
        public List<WordModel> WordsWithHints = new();
        public int Time;
        public int Score;
    }
}