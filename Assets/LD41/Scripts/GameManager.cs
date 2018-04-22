using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.LD41.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject EndGamePanel;
        [SerializeField] private GameObject WinGamePanel;
        [SerializeField] private TextMeshProUGUI TimerText;

        private static GameManager _instance;
        private bool _gameEnded;
        private float _startTime;
        private float _totalTime;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                this._startTime = Time.time;
                this.StartCoroutine("Timer");
                return;
            }

            GameObject.Destroy(this.gameObject);
        }

        public static GameManager Instance
        {
            get { return _instance; }
        }

        public void StartGame(string level)
        {
            SceneManager.LoadScene(level);
        }

        public void EndGame(bool success = false)
        {
                this._gameEnded = true;
            if (success)
            {
                GameManager.Instance.WinGamePanel.SetActive(true);
            }
            else
                GameManager.Instance.EndGamePanel.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ExitGame()
        {
            SceneManager.LoadScene("Menu");
        }

        private IEnumerator Timer()
        {
            while (!this._gameEnded)
            {
                yield return new WaitForEndOfFrame();

                var timer = Time.time - this._startTime;

                var m = Mathf.Floor(timer / 60);
                var s = Mathf.RoundToInt(timer % 60);

                string minutes, seconds;

                minutes = m.ToString();
                if (m < 10)
                    minutes = "0" + m;

                seconds = s.ToString();
                if (s < 10)
                    seconds = "0" + Mathf.RoundToInt(s);

                this.TimerText.text = String.Format("{0}:{1}", minutes, seconds);
            }

            this._totalTime = Time.time - this._startTime;
            //var md5 = MD5.Create();
            //var hash = System.Text.Encoding.ASCII.GetString(md5.ComputeHash(Encoding.ASCII.GetBytes("player" + this._totalTime + "SomeBigSecret")));

            //var w = new WWW(string.Format("https://grahamweldon.com/ld41/addscore.php?name=player&score={0}&hash={1}", this._totalTime, hash));
            var w = new WWW(string.Format("https://grahamweldon.com/ld41/addscore.php?name=player&score={0}", this._totalTime));
            yield return w;

            foreach (var r in w.responseHeaders)
            {
                Debug.Log(r.Key + " = " + r.Value);
            }

            Debug.Log(w.text);
        }
    }
}