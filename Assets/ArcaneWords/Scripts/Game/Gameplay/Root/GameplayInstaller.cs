using ArcaneWords.Scripts.Game.Gameplay.Level;
using ArcaneWords.Scripts.Game.Gameplay.Popups;
using ArcaneWords.Scripts.Game.Gameplay.Root.View;
using ArcaneWords.Scripts.Game.Gameplay.Typepaper;
using ArcaneWords.Scripts.Game.Gameplay.Typewriter;
using ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons;
using ArcaneWords.Scripts.Game.GameRoot.Bank;
using ArcaneWords.Scripts.Game.GameRoot.UI;
using UnityEngine;
using Zenject;

namespace ArcaneWords.Scripts.Game.Gameplay.Root
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private UIGameplayRootBinder _uiGameplayRootBinderPrefab;
        [SerializeField] private TypewriterView _typewriterViewPrefab;
        [SerializeField] private TypePaperView _typePaperViewPrefab;
        [SerializeField] private UIHintPopupView uiHintPopupPrefab;
        [SerializeField] private UITitleView _titleViewPrefab;
        [SerializeField] private UICoinsView _coinsViewPrefab;
        [SerializeField] private UIBulbsView _bulbsViewPrefab;
        [SerializeField] private UICoinPopupView uiCoinPopupPrefab;

        public override void InstallBindings()
        {
            InstallBankComponents();
            InstallTypewriterComponents();
            InstallTypePaperComponents();
            InstallUIComponents();
            Container.Bind<NextLevelStarter>().FromNew().AsSingle();
        }

        private void InstallTypewriterComponents()
        {
            Container.Bind<TypewriterView>().FromComponentInNewPrefab(_typewriterViewPrefab).AsSingle();
            Container.Bind<TypewriterViewModel>().FromNew().AsSingle();
            Container.Bind<TypewriterModel>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<ButtonsTimer>().AsSingle();
            Container.Bind<LevelTimer>().FromNew().AsSingle();
        }

        private void InstallTypePaperComponents()
        {
            Container.Bind<TypePaperView>().FromComponentInNewPrefab(_typePaperViewPrefab).AsSingle();
            Container.Bind<TypePaperViewModel>().FromNew().AsSingle();
            Container.Bind<TypePaperModel>().FromNew().AsSingle();
        }

        private void InstallBankComponents()
        {
            Container.Bind<BankModel>().FromNew().AsSingle();
            Container.Bind<BankViewModel>().FromNew().AsSingle();
            Container.Bind<UICoinsView>().FromComponentInNewPrefab(_coinsViewPrefab).AsSingle();
            Container.Bind<UIBulbsView>().FromComponentInNewPrefab(_bulbsViewPrefab).AsSingle();
        }

        private void InstallUIComponents()
        {
            Container.Bind<UIGameplayRootBinder>().FromComponentInNewPrefab(_uiGameplayRootBinderPrefab).AsSingle();
            Container.Bind<UIHintPopupView>().FromComponentInNewPrefab(uiHintPopupPrefab).AsSingle();
            Container.Bind<UITitleView>().FromComponentInNewPrefab(_titleViewPrefab).AsSingle();
            Container.Bind<UICoinPopupView>().FromComponentInNewPrefab(uiCoinPopupPrefab).AsSingle();
        }
}
}