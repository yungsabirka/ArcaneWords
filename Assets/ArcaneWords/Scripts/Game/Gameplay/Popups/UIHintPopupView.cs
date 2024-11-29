using ArcaneWords.Scripts.Game.Gameplay.Typepaper.Word;
using ArcaneWords.Scripts.Game.Gameplay.Typewriter;
using ArcaneWords.Scripts.Game.GameRoot.Bank;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ArcaneWords.Scripts.Game.Gameplay.Popups
{
    public class UIHintPopupView : MonoBehaviour
    {
        [SerializeField] private Image _blurImage;
        [SerializeField] private TextMeshProUGUI _word;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _priceText;
        
        public Subject<Unit> OnPopupShown = new();
        public Subject<Unit> OnPopupHidden = new();
        public Subject<string> OnDescriptionBought= new();
        
        private BankViewModel _bankViewModel;
        private WordViewModel _currentWord;
        
        [Inject]
        private void Construct(BankViewModel bankViewModel)
        {
            _bankViewModel = bankViewModel;
        }

        private void Start()
        {
            _buyButton.onClick.AddListener(OnBuyButtonClicked);
            _closeButton.onClick.AddListener(Hide);
        }
        
        private void OnBuyButtonClicked()
        {
            if (_bankViewModel.TryToSpendBulbs(_currentWord.Word.Length))
            {
                _currentWord.AllowDescription();
                SetHintAvailability(true);
                OnDescriptionBought.OnNext(_currentWord.Word);
            }
        }
        
        public void SetWord(WordViewModel word)
        {
            _currentWord = word;
            _word.text = word.IsWordAvailable.CurrentValue ? word.Word : new string('-', word.Word.Length);
            _description.text = word.Description;
            _priceText.text = word.Word.Length.ToString();
            SetHintAvailability(word.IsDesctiptionAvailable.CurrentValue);
        }

        private void SetHintAvailability(bool isHintAvailable)
        {
            _description.gameObject.SetActive(isHintAvailable);
            _blurImage.gameObject.SetActive(!isHintAvailable);
            _buyButton.gameObject.SetActive(!isHintAvailable);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            OnPopupShown?.OnNext(Unit.Default);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            OnPopupHidden?.OnNext(Unit.Default);
        }
    }
}