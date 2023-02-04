using JamKit;
using UnityEngine;
using TMPro;

namespace Game
{
    public class GameUi : UiBase
    {
        [SerializeField] private FlashInfo _openFlashInfo;

        public TMP_Text emailFrom;
        public TMP_Text emailTo;
        public TMP_Text emailSubject;
        public TMP_Text emailContent;

        [SerializeField] private GameObject _notificationRoot;
        [SerializeField] private GameObject _confirmationRoot;
        [SerializeField] private TextMeshProUGUI _confirmationText;

        void Start()
        {
            Flash(_openFlashInfo);
        }

        public void SetEmail(Email email)
        {
            emailFrom.text = email.From;
            emailTo.text = email.To;
            emailSubject.text = email.Subject;
            emailContent.text = email.MessageBody;
        }

        public void ClearEmail()
        {
            emailFrom.text = "";
            emailTo.text = "";
            emailSubject.text = "";
            emailContent.text = "";
        }

        public void SetConfirmation(EmailResult value)
        {
            _notificationRoot.SetActive(false);
            _confirmationRoot.SetActive(true);

            _confirmationText.text = value == EmailResult.Yes ? "MESSAGE FORWARDED" : "MESSAGE DELETED";
        }

        public void ClearConfirmation()
        {
            _confirmationRoot.SetActive(false);
        }

        public void SetNotification()
        {
            _notificationRoot.SetActive(true);
            _confirmationRoot.SetActive(false);
        }

        public void ClearNotification()
        {
            _notificationRoot.SetActive(false);
        }
    }
}