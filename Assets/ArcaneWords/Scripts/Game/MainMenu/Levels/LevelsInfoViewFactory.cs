using ArcaneWords.Scripts.Game.GameRoot.Level;
using ArcaneWords.Scripts.Game.MainMenu.Root.View;
using UnityEngine;
using Zenject;

namespace ArcaneWords.Scripts.Game.MainMenu.Levels
{
    public class LevelsInfoViewFactory : MonoBehaviour
    {
        [SerializeField] private LevelInfoView _levelInfoViewPrefab;
        [SerializeField] private Transform _levelsInfoContainer;
        [SerializeField] private Sprite _bronzeMedalSprite;
        [SerializeField] private Sprite _silverMedalSprite;
        [SerializeField] private Sprite _goldMedalSprite;
        
        private LevelsHandler _levelsHandler;
        private UIMainMenuRootBinder _uiMainMenuRootBinder;
        private UIBuyLevelPopup _uiBuyLevelPopup;

        [Inject]
        private void Construct(LevelsHandler levelsHandler, UIMainMenuRootBinder uiMainMenuRootBinder, UIBuyLevelPopup uiBuyLevelPopup)
        {
            _levelsHandler = levelsHandler;
            _uiMainMenuRootBinder = uiMainMenuRootBinder;
            _uiBuyLevelPopup = uiBuyLevelPopup;
        }

        public void Init()
        {
            CreateLevelInfoContainerViews();
        }

        private void CreateLevelInfoContainerViews()
        {
            var levelNames = _levelsHandler.GetLevelNames();

            for (int i = 0; i < _levelsInfoContainer.childCount; i++)
            {
                var levelInfoShelfView = _levelsInfoContainer.GetChild(i).GetComponent<LevelInfoShelfView>();
                if (i > levelNames.Count - 1)
                {
                    levelInfoShelfView.InitEmptyShelf(_uiBuyLevelPopup);
                    continue;
                }

                var levelInfoView = Instantiate(_levelInfoViewPrefab, levelInfoShelfView.transform);
                var wordsAmount = _levelsHandler.GetLevelWordsAmountByName(levelNames[i]);
                var levelInfo = _levelsHandler.GetLevelInfoByName(levelNames[i]);
                var percentScore = levelInfo.GuessedWords.Count * 100 / wordsAmount;

                levelInfoView.SetLevelName(levelNames[i]);
                if (percentScore < 100)
                {
                    var medalSprite = GetMedalSprite(percentScore);
                    levelInfoView.SetMedalImage(medalSprite);
                    levelInfoView.SetLevelPercentScore(percentScore);
                }
                else
                {
                    levelInfoView.SetLevelCompleteState();
                }

                levelInfoShelfView.InitLevelShelf(levelInfoView, _uiMainMenuRootBinder.HandleGoToGameplayButtonClick);
            }
        }

        private Sprite GetMedalSprite(int percentScore)
        {
            return percentScore switch
            {
                >= 30 and <= 59 => _bronzeMedalSprite,
                >= 60 and <= 89 => _silverMedalSprite,
                >= 90 and <= 99 => _goldMedalSprite,
                _ => null
            };
        }
    }
}