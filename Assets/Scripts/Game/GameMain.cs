using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using JamKit;
using UnityEngine.SceneManagement;

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
        private enum GameState
        {
            Email,
            Wait,
            Notification,
            Confirmation,
        }

        public GameUi gameUi;

        private int _score = 0;
        private EmailResult? _lastResponse = null;

        private int _currentEmailIndex = 0;
        private Email[] _emails;
        private GameState _currentState = GameState.Email;

        private void Start()
        {
            TextAsset[] emailAssets = Resources.LoadAll<TextAsset>("Emails");
            List<Email> emailsList = new List<Email>();
            foreach (TextAsset emailAsset in emailAssets)
            {
                Email email = JsonUtility.FromJson<Email>(emailAsset.text);
                emailsList.Add(email);
            }

#if UNITY_EDITOR
            foreach (Email email1 in emailsList)
            {
                foreach (Email email2 in emailsList)
                {
                    if (email1 != email2 && email1.Index == email2.Index)
                    {
                        Debug.LogError($"Duplicate index! {email1.Index}. Subjects: [{email1.Subject}] [{email2.Subject}]");
                    }
                }
            }
#endif

            _emails = emailsList.OrderBy(x => x.Index).ToArray();

            _currentEmailIndex = 0;
            SetState(GameState.Email);
        }

        private void SetEmail(Email email)
        {
            gameUi.SetEmail(email);
        }

        public void ResponseGiven(int resultInt)
        {
            Email email = _emails[_currentEmailIndex];
            EmailResult result = (EmailResult)resultInt;
            if (result == EmailResult.Yes)
            {
                Sfx.Instance.PlayRandom("Forward");
                _score += email.ForwardScore;
            }
            else
            {
                Sfx.Instance.PlayRandom("Delete");
                _score += email.ReportScore;
            }

            _lastResponse = result;
            gameUi.ClearEmail();

            SetState(GameState.Confirmation);
        }

        public void NotificationClicked()
        {
            _currentEmailIndex++;

            Sfx.Instance.Play("ClickMenu");
            SetState(GameState.Email);
            gameUi.ClearNotification();
        }

        public void ConfirmationClicked()
        {
            // End game
            if (_currentEmailIndex == _emails.Length - 1)
            {
                PlayerPrefs.SetInt("root_score", _score);
                gameUi.FadeOut();
                CoroutineStarter.RunDelayed(1.0f, () =>
                {
                    SceneManager.LoadScene("End");
                });

                return;
            }

            Sfx.Instance.Play("ClickMenu");
            SetState(GameState.Wait);
            gameUi.ClearConfirmation();
        }

        private void SetState(GameState newState)
        {
            _currentState = newState;
            switch (_currentState)
            {
                case GameState.Email:
                    SetEmail(_emails[_currentEmailIndex]);
                    break;
                case GameState.Wait:
                    CoroutineStarter.RunDelayed(UnityEngine.Random.Range(1.0f, 1.5f), () =>
                    {
                        Sfx.Instance.Play("Notification");
                        SetState(GameState.Notification);
                    });
                    break;
                case GameState.Notification:
                    gameUi.SetNotification();
                    break;
                case GameState.Confirmation:
                    gameUi.SetConfirmation(_lastResponse.Value);

                    _lastResponse = null;
                    break;
            }

        }
    }
}