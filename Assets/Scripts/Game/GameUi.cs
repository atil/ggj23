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

		void Start()
        {
            Flash(_openFlashInfo);
        }

        public void SetEmail(Email email)
		{
            emailFrom.text = email.from;
            emailTo.text = email.to;
            emailSubject.text = email.subject;
            emailContent.text = email.messageBody;
		}
    }
}