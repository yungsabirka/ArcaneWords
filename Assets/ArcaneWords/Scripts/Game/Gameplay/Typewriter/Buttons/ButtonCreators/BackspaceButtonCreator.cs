using ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.Views;
using UnityEngine;

namespace ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.ButtonCreators
{
    public class BackspaceButtonCreator : ButtonCreator
    {
        private const string PrefabName = "UIBackspaceButton";
        private const float ButtonHoldTime = 0.3f;
        
        public override TypewriterButtonView CreateButton()
        {
            if(ButtonViewPrefab == null)
                ButtonViewPrefab = Resources.Load<TypewriterButtonView>($"{PrefabsFolderPath}/{PrefabName}");

            var backspaceButton = Object.Instantiate(ButtonViewPrefab);

            backspaceButton.Init(ButtonHoldTime);
            return backspaceButton;
        }
    }
}