using ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.Views;
using UnityEngine;

namespace ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.ButtonCreators
{
    public class LetterButtonCreator : ButtonCreator
    {
        private const string PrefabName = "UILetterButton";
        private const float ButtonHoldTime = 1.0f;
        
        public override TypewriterButtonView CreateButton()
        {
            if(ButtonViewPrefab == null)
                ButtonViewPrefab = Resources.Load<TypewriterButtonView>($"{PrefabsFolderPath}/{PrefabName}");

            var letterButton = Object.Instantiate(ButtonViewPrefab);
            letterButton.Init(ButtonHoldTime);
            return letterButton;
        }
    }
}