using ObservableCollections;
using TMPro;
using UnityEngine;

namespace ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.Views
{
    public class LetterButtonView : TypewriterButtonView
    {
        [SerializeField] private TextMeshProUGUI _letterView;

        public char Letter => _letter;
        private char _letter;

        public void SetLetter(char letter)
        {
            _letter = letter;
            _letterView.SetText(_letter.ToString());
        }

        public void EnableLetterView()
        {
            _letterView.gameObject.SetActive(true);
        }

        public void DisableLetterView()
        {
            _letterView.gameObject.SetActive(false);
        }

        public override void InteractWithText(ObservableList<char> text)
        {
            text.Add(_letter);
        }
        
        protected override void OnButtonClicked()
        {
            if (IsPressed || IsActive == false)
                return;
            
            Press();
        }
    }
}