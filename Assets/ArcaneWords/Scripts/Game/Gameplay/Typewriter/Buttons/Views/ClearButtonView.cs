using DG.Tweening;
using ObservableCollections;

namespace ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.Views
{
    public class ClearButtonView : TypewriterButtonView
    {
        public override void InteractWithText(ObservableList<char> text)
        {
            if (text.Count > 0)
                text.Clear();
        }

        protected override void OnButtonClicked()
        {
            if (IsPressed || IsActive == false)
                return;
            
            Press().OnComplete(() => Release());
        }
    }
}