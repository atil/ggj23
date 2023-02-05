using JamKit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class SplashUi : UiBase
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private FlashInfo _openFlashInfo;
        [SerializeField] private FlashInfo _closeFlashInfo;

        void Start()
        {
            Sfx.Instance.Play("Splash");
            Flash(_openFlashInfo);
        }

        public void OnClickedPlayButton()
        {
            Sfx.Instance.PlayRandom("Click");
            _playButton.interactable = false;
            Flash(_closeFlashInfo, () => SceneManager.LoadScene("Game"));
        }
    }
}