using ArcaneWords.Scripts.Game.Gameplay.Level;
using ArcaneWords.Scripts.Game.GameRoot.Level;
using ArcaneWords.Scripts.Game.Gameplay.Popups;
using ArcaneWords.Scripts.Game.Gameplay.Root.View;
using ArcaneWords.Scripts.Game.Gameplay.Typepaper;
using ArcaneWords.Scripts.Game.Gameplay.Typewriter;
using ArcaneWords.Scripts.Game.GameRoot.Bank;
using ArcaneWords.Scripts.Game.GameRoot.UI;
using ArcaneWords.Scripts.Game.MainMenu.Root;
using DG.Tweening;
using R3;
using UnityEngine;
using Zenject;

namespace ArcaneWords.Scripts.Game.Gameplay.Root
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _uiGameplayBgPrefab;

        private UIRootView _uiRootView;
        private UIGameplayRootBinder _uiGameplayRootBinderPrefab;
        private LevelsHandler _levelsHandler;
        private TypewriterView _typewriterView;
        private TypewriterViewModel _typewriterViewModel;
        private TypewriterModel _typewriterModel;
        private TypePaperView _typePaperView;
        private TypePaperViewModel _typePaperViewModel;
        private TypePaperModel _typePaperModel;
        private UIHintPopupView _uiHintPopupView;
        private UITitleView _titleView;
        private UICoinsView _uiCoinsView;
        private UIBulbsView _bulbsView;
        private BankModel _bankModel;
        private UICoinPopupView _uiCoinPopupView;
        private NextLevelStarter _nextLevelStarter;

        [Inject]
        private void Construct(UIRootView uiRootView, UIGameplayRootBinder uiGameplayRootBinderPrefab,
            TypewriterView typewriterView, TypewriterViewModel typewriterViewModel, TypewriterModel typewriterModel,
            TypePaperView typePaperView, TypePaperViewModel typePaperViewModel, LevelsHandler levelsHandler,
            TypePaperModel typePaperModel, UIHintPopupView uiHintPopupView, UITitleView titleView,
            UICoinsView coinsView, UIBulbsView bulbsView, BankModel bankModel, UICoinPopupView uiCoinPopupView,
            NextLevelStarter nextLevelStarter)
        {
            _uiRootView = uiRootView;
            _uiGameplayRootBinderPrefab = uiGameplayRootBinderPrefab;
            _levelsHandler = levelsHandler;
            _typewriterView = typewriterView;
            _typewriterViewModel = typewriterViewModel;
            _typewriterModel = typewriterModel;
            _typePaperView = typePaperView;
            _typePaperViewModel = typePaperViewModel;
            _typePaperModel = typePaperModel;
            _uiHintPopupView = uiHintPopupView;
            _titleView = titleView;
            _uiCoinsView = coinsView;
            _bulbsView = bulbsView;
            _bankModel = bankModel;
            _uiCoinPopupView = uiCoinPopupView;
            _nextLevelStarter = nextLevelStarter;
        }

        public Observable<GameplayExitParams> Run(GameplayEnterParams enterParams)
        {
            CreateAndAttachUI();

            _titleView.SetTitle(enterParams.LevelName);
            _levelsHandler.SetCurrentLevel(enterParams.LevelName);

            InitTypewriter(enterParams.LevelName);
            InitBank();
            InitTypePaper();
            
            _nextLevelStarter.Init();

            var exitSceneSignalsSubject = new Subject<Unit>();
            _uiGameplayRootBinderPrefab.Bind(exitSceneSignalsSubject);

            var mainMenuEnterParams = new MainMenuEnterParams();
            var exitParams = new GameplayExitParams(mainMenuEnterParams);
            var exitToMainMenuSceneSignal = exitSceneSignalsSubject.Select(_ => exitParams);
            return exitToMainMenuSceneSignal;
        }

        private void CreateAndAttachUI()
        {
            var uiGameplayBg = Instantiate(_uiGameplayBgPrefab);

            _uiRootView.AttachSceneUI(uiGameplayBg, UILayer.HUD);
            _uiRootView.AttachSceneUI(_uiGameplayRootBinderPrefab.gameObject, UILayer.HUD);
            _uiRootView.AttachSceneUI(_typewriterView.gameObject, UILayer.HUD);
            _uiRootView.AttachSceneUI(_titleView.gameObject, UILayer.HUD);
            _uiRootView.AttachSceneUI(_uiHintPopupView.gameObject, UILayer.Popup);
            _uiRootView.AttachSceneUI(_uiCoinPopupView.gameObject, UILayer.FXOverPopup);
            _uiRootView.AttachSceneUI(_typePaperView.gameObject, UILayer.HUD);
            _uiRootView.AttachSceneUI(_uiCoinsView.gameObject, UILayer.HUD);
            _uiHintPopupView.Hide();
            _uiCoinPopupView.Hide();
        }

        private void InitTypePaper()
        {
            _typePaperView.Init();
            _typePaperViewModel.Init();
            _typePaperModel.Init();
        }

        private void InitTypewriter(string levelName)
        {
            _typewriterModel.Init();
            _typewriterViewModel.Init();
            _typewriterView.Init(levelName);
        }

        private void InitBank()
        {
            _bankModel.Init();
            _uiCoinsView.Init();
            _bulbsView.Init();
        }

        private void OnDestroy()
        {
            DOTween.KillAll();
            if (_bankModel.Inited)
                _bankModel.SaveBankData();

            _nextLevelStarter.Dispose();
            _levelsHandler.SaveCurrentLevel();
        }
    }
}