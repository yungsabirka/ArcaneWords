using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneWords.Scripts.Game.MainMenu.Levels
{
    [RequireComponent(typeof(Button))]
    public class LevelInfoShelfView: MonoBehaviour
    {
        [SerializeField] private GameObject _keyHole;
        [SerializeField] private Transform _levelInfoContainer;
        
        public void InitLevelShelf(LevelInfoView levelInfoView, Action<string> handleGoToGameplayButtonClick)
        {
            var button = GetComponent<Button>();
            _keyHole.SetActive(false);
            levelInfoView.transform.SetParent(_levelInfoContainer, false);
            if (levelInfoView.IsLevelCompleted == false)
                button.onClick.AddListener(() => handleGoToGameplayButtonClick.Invoke(levelInfoView.LevelName));
        }

        public void InitEmptyShelf(UIBuyLevelPopup buyLevelPopup)
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(buyLevelPopup.Show);
        }
    }
}