using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.LD41.Scripts
{
    public class EndZone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.layer != 8)
                return;

            GameManager.Instance.EndGame(true);
        }
    }
}