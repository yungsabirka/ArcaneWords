using ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.Views;
using UnityEngine;

namespace ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.ButtonCreators
{
    public class ClearButtonCreator : ButtonCreator
    {
        private const string PrefabName = "UIClearButton"; 
        private const float ButtonHoldTime = 0.3f;
        
        public override TypewriterButtonView CreateButton()
        {
            if(ButtonViewPrefab == null)
                ButtonViewPrefab = Resources.Load<TypewriterButtonView>($"{PrefabsFolderPath}/{PrefabName}");
            
            var clearButton = Object.Instantiate(ButtonViewPrefab);

            clearButton.Init(ButtonHoldTime);
            return clearButton;
        }
    }
}