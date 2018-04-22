using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.LD41.Scripts
{
    public class Player : MonoBehaviour
    {
        private void OnCollisionEnter(Collision c)
        {
            var pol = c.gameObject.GetComponent<Police>();
            if (pol == null)
                return;

            // Nothing to see here, move along...
            if (pol.CurrentState != PoliceState.Chasing)
                return;

            GameManager.Instance.EndGame();
        }

        private void OnTriggerEnter(Collider c)
        {
            //Debug.Log("There is a " + c.gameObject.name + " inside me.");
        }
    }
}