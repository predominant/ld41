using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.LD41.Scripts
{
    public class AutoSceneTransition : MonoBehaviour
    {
        [SerializeField] private float Time;
        [SerializeField] private string TargetScene;

        private void Start()
        {
            this.StartCoroutine("Countdown");
        }

        private IEnumerator Countdown()
        {
            var scene = this.TargetScene;
            yield return new WaitForSecondsRealtime(this.Time);
            SceneManager.LoadScene(scene);
        }
    }
}