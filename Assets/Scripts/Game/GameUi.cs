﻿using JamKit;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game
{
    public class GameUi : UiBase
    {
        [SerializeField] private FlashInfo _openFlashInfo;
        [SerializeField] private FlashInfo _closeFlashInfo;

        public TMP_Text emailFrom;
        public TMP_Text emailTo;
        public TMP_Text emailSubject;
        public TMP_Text emailContent;

        [SerializeField] private Button _emailForwardButton;
        [SerializeField] private Button _emailDeleteButton;

        [SerializeField] private GameObject _feedbackRoot;
        [SerializeField] private GameObject _notificationRoot;
        [SerializeField] private GameObject _confirmationRoot;
        [SerializeField] private TextMeshProUGUI _confirmationText;
        [SerializeField] private Button _confirmationButton;
        [SerializeField] private GameObject _logoRoot;

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
            _emailForwardButton.interactable = true;
            _emailDeleteButton.interactable = true;

            _logoRoot.SetActive(false);
        }

        public void ClearEmail()
        {
            emailFrom.text = "";
            emailTo.text = "";
            emailSubject.text = "";
            emailContent.text = "";
            _emailForwardButton.interactable = false;
            _emailDeleteButton.interactable = false;
        }

        public void SetFeedback()
        {
            _feedbackRoot.SetActive(true);
            _logoRoot.SetActive(false);

        }

        public void ClearFeedback()
        {
            _feedbackRoot.SetActive(false);
        }

        public void SetConfirmation(EmailResult value)
        {
            _notificationRoot.SetActive(false);
            _confirmationRoot.SetActive(true);
            _logoRoot.SetActive(false);
            _confirmationButton.interactable = true;

            _confirmationText.text = value == EmailResult.Yes ? "MESSAGE FORWARDED" : "MESSAGE DELETED";
        }

        public void ClearConfirmation()
        {
            _confirmationRoot.SetActive(false);
            _confirmationButton.interactable = false;
        }

        public void SetNotification()
        {
            _notificationRoot.SetActive(true);
            _confirmationRoot.SetActive(false);
            _logoRoot.SetActive(false);
        }

        public void ClearNotification()
        {
            _notificationRoot.SetActive(false);
        }

        public void SetLogo()
        {
            _logoRoot.SetActive(true);
        }

        public void FadeOut()
        {
            Flash(_closeFlashInfo);
            _confirmationButton.interactable = false;
        }
    }
}