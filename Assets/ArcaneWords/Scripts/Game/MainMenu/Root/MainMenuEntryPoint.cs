using ArcaneWords.Scripts.Game.GameRoot.Bank;
using ArcaneWords.Scripts.Game.GameRoot.UI;
using ArcaneWords.Scripts.Game.MainMenu.Levels;
using ArcaneWords.Scripts.Game.MainMenu.Root.View;
using R3;
using UnityEngine;
using Zenject;

namespace ArcaneWords.Scripts.Game.MainMenu.Root
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _uiMainMenuBgPrefab;
        
        private UIMainMenuRootBinder _mainMenuRootBinder;
        private UIRootView _uiRootView;
        private LevelsInfoViewFactory _levelsInfoFactory;
        private UITitleView _titleView;
        private UICoinsView _coinsView;
        private BankModel _bankModel;
        private UIBuyLevelPopup _buyLevelPopupPrefab;

        [Inject]
        private void Construct(UIRootView uiRootView, UIMainMenuRootBinder mainMenuRootBinder, LevelsInfoViewFactory levelsInfoFactory,
            UITitleView titleView, UICoinsView coinsView, BankModel bankModel, UIBuyLevelPopup buyLevelPopupPrefab)
        {
            _uiRootView = uiRootView;
            _mainMenuRootBinder = mainMenuRootBinder;
            _levelsInfoFactory = levelsInfoFactory;
            _titleView = titleView;
            _coinsView = coinsView;
            _bankModel = bankModel;
            _buyLevelPopupPrefab = buyLevelPopupPrefab;
        }

        public Observable<MainMenuExitParams> Run(MainMenuEnterParams enterParams)
        {
            CreateAndAttachUI();
            _titleView.SetTitle("Select level");
            
            InitBank();
            _levelsInfoFactory.Init();
            
            var exitSignalSubject = new Subject<MainMenuExitParams>();
            _mainMenuRootBinder.Bind(exitSignalSubject);
            
            Observable<MainMenuExitParams> exitToGameplaySceneSignal = exitSignalSubject;

            return exitToGameplaySceneSignal;
        }
        
        private void CreateAndAttachUI()
        {
            var uiMainMenuBg = Instantiate(_uiMainMenuBgPrefab);

            _uiRootView.AttachSceneUI(_mainMenuRootBinder.gameObject, UILayer.HUD);
            _uiRootView.AttachSceneUI(uiMainMenuBg.gameObject, UILayer.HUD);
            _uiRootView.AttachSceneUI(_levelsInfoFactory.gameObject, UILayer.HUD);
            _uiRootView.AttachSceneUI(_titleView.gameObject, UILayer.HUD);
            _uiRootView.AttachSceneUI(_buyLevelPopupPrefab.gameObject, UILayer.Popup);
            _buyLevelPopupPrefab.Hide();
        }
        
        private void InitBank()
        {
            _uiRootView.AttachSceneUI(_coinsView.gameObject, UILayer.HUD);
            if(_bankModel.Inited)
                _bankModel.SaveBankData();
            _bankModel.Init();
            _coinsView.Init();
        }
    }
}