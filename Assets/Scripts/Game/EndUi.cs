using JamKit;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class EndUi : UiBase
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private FlashInfo _openFlashInfo;
        [SerializeField] private FlashInfo _closeFlashInfo;

        [SerializeField] private TextMeshProUGUI _gameOverText;

        void Start()
        {
            Flash(_openFlashInfo);

            int score = PlayerPrefs.GetInt("root_score");
            // todo: set text here
        }

        public void OnClickedPlayButton()
        {
            _playButton.interactable = false;
            Flash(_closeFlashInfo, () => SceneManager.LoadScene("Game"));
        }
    }
}