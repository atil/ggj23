using JamKit;
using System.Collections;
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
        [SerializeField] private TextMeshProUGUI _buttonText;

        [SerializeField] private TextMeshProUGUI _gameOverText;
        [SerializeField] private GameObject _splat;

        void Start()
        {
            int score = PlayerPrefs.GetInt("root_score");

            // TODO TEMP
            score = -1;
            const int WinScore = 0;

            if (score >= WinScore)
            {
                _gameOverText.text = "You've done well Rooter. <color=#f00008ff>MOTHER</color> will be visiting you very soon.\nWait for our instructions.";
                _buttonText.text = "Long Live <color=#f00008ff>TAMASH!</color>";
                Flash(_openFlashInfo);
            }
            else
            {
                _gameOverText.text = "I knew the Sub Rooter was a <color=#f00008ff>MAGGOT</color> all along. <color=#f00008ff>EMBLAZING</color> it was a delight.";
                _buttonText.text = "Long Live <color=#f00008ff>TAMASH!</color>";

                CoroutineStarter.RunDelayed(2f, () =>
                {
                    _splat.SetActive(true);
                    Sfx.Instance.Play("Shot");
                    CoroutineStarter.RunDelayed(2f, () =>
                    {
                        Flash(_openFlashInfo);
                    });
                });
            }
        }

        public void OnClickedPlayButton()
        {
            _playButton.interactable = false;
            Flash(_closeFlashInfo, () => SceneManager.LoadScene("Game"));
        }
    }
}