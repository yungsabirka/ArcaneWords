using DG.Tweening;
using ObservableCollections;

namespace ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.Views
{
    public class BackspaceButtonView : TypewriterButtonView
    {
        public override void InteractWithText(ObservableList<char> text)
        {
            if (text.Count > 0)
                text.RemoveAt(text.Count - 1);
        }

        protected override void OnButtonClicked()
        {
            if (IsPressed || IsActive == false)
                return;

            Press().OnComplete(() => Release());
        }
    }
}