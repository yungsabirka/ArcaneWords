using TMPro;
using UnityEngine;

namespace ArcaneWords.Scripts.Game.GameRoot.UI
{
    public class UITitleView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;

        public void SetTitle(string title)
        {
            _titleText.text = title;
        }
    }
}