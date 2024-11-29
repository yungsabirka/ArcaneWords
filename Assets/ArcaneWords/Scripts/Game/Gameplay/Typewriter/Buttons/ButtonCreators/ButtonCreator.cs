using ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.Views;
using UnityEngine;

namespace ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.ButtonCreators
{
    public abstract class ButtonCreator
    {
        protected const string PrefabsFolderPath = "Prefabs/UI/TypewriterButtons";
        protected TypewriterButtonView ButtonViewPrefab;
        
        public abstract TypewriterButtonView CreateButton();
    }
}