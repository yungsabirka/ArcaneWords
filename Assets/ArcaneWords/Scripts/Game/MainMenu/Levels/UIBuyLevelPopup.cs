using UnityEngine;

namespace ArcaneWords.Scripts.Game.MainMenu.Levels
{
    public class UIBuyLevelPopup : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}