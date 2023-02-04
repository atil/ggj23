using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Game
{
	[Serializable]
	public enum EmailResult
	{
		Yes = 0,
		No = 1,
	}
    public class GameMain : MonoBehaviour
    {
		public GameUi gameUi;

		int currentEmailIndex = 0;
		Email[] emails;
		private void Start()
		{
			TextAsset[] emailAssets = Resources.LoadAll<TextAsset>("Emails");
			List<Email> emailsList = new List<Email>();
			foreach(TextAsset emailAsset in emailAssets)
			{
				Email email = JsonUtility.FromJson<Email>(emailAsset.text);
				emailsList.Add(email);
			}

			emails = emailsList.OrderBy(x => x.index).ToArray();

			currentEmailIndex = 0;
			SetEmail(emails[currentEmailIndex]);
		}

		public void SetEmail(Email email)
		{
			gameUi.SetEmail(email);
		}

		public void ResponseGiven(int result)
		{
			Email email = emails[currentEmailIndex];
			if(email.expectedResult == (EmailResult)result)
			{

			}
			else
			{

			}
			currentEmailIndex++;
			SetEmail(emails[currentEmailIndex]);
		}
	}
}