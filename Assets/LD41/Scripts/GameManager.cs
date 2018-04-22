using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.LD41.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject EndGamePanel;
        [SerializeField] private GameObject WinGamePanel;

        private static GameManager _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
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
            if (success)
                GameManager.Instance.WinGamePanel.SetActive(true);
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
    }
}