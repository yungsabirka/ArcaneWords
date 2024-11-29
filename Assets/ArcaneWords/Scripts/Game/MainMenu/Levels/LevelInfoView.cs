using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneWords.Scripts.Game.MainMenu.Levels
{
    public class LevelInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelNameText;
        [SerializeField] private TextMeshProUGUI _levelPercentScoreText;
        [SerializeField] private GameObject _levelScorePanel;
        [SerializeField] private Image _levelCompleteImage;
        [SerializeField] private Image _levelMedalImage;

        public bool IsLevelCompleted { get; private set; }
        public string LevelName => _levelNameText.text;
        
        public void SetLevelName(string levelName)
        {
            _levelNameText.text = levelName;
        }

        public void SetLevelPercentScore(int levelPercentScore)
        {
            _levelPercentScoreText.text = $"{levelPercentScore}%";
        }

        public void SetMedalImage(Sprite medal)
        {
            _levelMedalImage.sprite = medal;
        }

        public void SetLevelCompleteState()
        {
            _levelCompleteImage.gameObject.SetActive(true);
            _levelScorePanel.SetActive(false);
            IsLevelCompleted = true;
        }
    }
}
