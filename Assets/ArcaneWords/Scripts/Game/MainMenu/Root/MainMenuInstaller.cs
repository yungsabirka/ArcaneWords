using ArcaneWords.Scripts.Game.GameRoot.Bank;
using ArcaneWords.Scripts.Game.GameRoot.UI;
using ArcaneWords.Scripts.Game.MainMenu.Levels;
using ArcaneWords.Scripts.Game.MainMenu.Root.View;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace ArcaneWords.Scripts.Game.MainMenu.Root
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private UIMainMenuRootBinder _mainMenuRootBinder;
        [SerializeField] private LevelsInfoViewFactory levelsInfoPrefab;
        [SerializeField] private UITitleView _titleViewPrefab;
        [SerializeField] private UICoinsView _coinsViewPrefab;
        [SerializeField] private UIBuyLevelPopup _buyLevelPopupPrefab;

        public override void InstallBindings()
        {
            Container.Bind<UIMainMenuRootBinder>().FromComponentInNewPrefab(_mainMenuRootBinder).AsSingle();
            Container.Bind<LevelsInfoViewFactory>().FromComponentInNewPrefab(levelsInfoPrefab).AsSingle();
            Container.Bind<UITitleView>().FromComponentInNewPrefab(_titleViewPrefab).AsSingle();
            Container.Bind<UICoinsView>().FromComponentInNewPrefab(_coinsViewPrefab).AsSingle();
            Container.Bind<UIBuyLevelPopup>().FromComponentInNewPrefab(_buyLevelPopupPrefab).AsSingle();

            Container.Bind<BankModel>().FromNew().AsSingle();
            Container.Bind<BankViewModel>().FromNew().AsSingle();
        }
    }
}