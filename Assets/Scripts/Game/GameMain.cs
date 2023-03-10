using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using JamKit;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

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
            Login,
            Email,
            Feedback,
            Wait,
            Notification,
            Confirmation,
        }

        public GameUi gameUi;

        private int _score = 0;
        private EmailResult? _lastResponse = null;

        private int _currentScreenIndex = 0;
        private int _currentEmailIndex = 0;
        private Email[] _emails;
        private GameState _currentState;

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

            for (int i = 0; i < _emails.Length; i++)
            {
                var words = _emails[i].MessageBody.Split(' ');
                for (int j = 0; j < words.Length; j++)
                {
                    if (words[j].Length <= 0)
                    {
                        continue;
                    }
                    if (words[j][0] == '#')
                    {
                        words[j] = "<color=#f00008ff>" + words[j].Substring(1) + "</color>";
                    }
                }
                _emails[i].MessageBody = String.Join(" ", words);
            }

            _currentEmailIndex = -1;
            SetState(GameState.Login);
        }

        private void SetEmail(Email email)
        {
            gameUi.SetEmail(email);
        }

        private void SetFeedback(int week)
        {
            bool isPositive = _score > _currentEmailIndex / 2.0f;
            int follower = isPositive ? (int)Mathf.Pow(2, _score) + 314 : 1;
            gameUi.SetFeedback(week, isPositive, follower);
        }

        public void LoginClicked()
        {
            Sfx.Instance.Play("ClickLogin");
            SetState(GameState.Wait);
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

        public void VoidResponseGiven()
        {
            Sfx.Instance.PlayRandom("Forward");
            gameUi.ClearFeedback();
            SetState(GameState.Wait);
        }

        public void NotificationClicked()
        {
            _currentScreenIndex++;
            Sfx.Instance.Play("ClickMenu");
            if (_currentScreenIndex % 4 == 0)
            {
                SetState(GameState.Feedback);
            }
            else
            {
                _currentEmailIndex++;
                SetState(GameState.Email);
            }
            gameUi.ClearNotification();
        }

        public void ConfirmationClicked()
        {
            // End game
            if (_currentEmailIndex == _emails.Length - 1)
            {
                PlayerPrefs.SetInt("root_score", _score);
                gameUi.FadeOut();
                CoroutineStarter.RunDelayed(1.0f /* comes from UI closeFlashInfo */, () =>
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
                case GameState.Login:
                    gameUi.SetLogin();
                    break;
                case GameState.Email:
                    SetEmail(_emails[_currentEmailIndex]);
                    break;
                case GameState.Feedback:
                    SetFeedback(_currentScreenIndex / 4);
                    break;
                case GameState.Wait:
                    gameUi.SetLogo();
                    CoroutineStarter.RunDelayed(UnityEngine.Random.Range(1.0f, 1.5f), () =>
                    {
                        if ((_currentScreenIndex + 1) % 4 == 0) // Feedback
                        {
                            Sfx.Instance.Play("Notification2");
                        }
                        else
                        {
                            Sfx.Instance.Play("Notification");
                        }

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