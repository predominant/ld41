using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.LD41.Scripts
{
    public enum MouseButton
    {
        Left = 0,
        Right = 1,
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshDebug : MonoBehaviour
    {
        [SerializeField] private MouseButton MouseButton = MouseButton.Left;

        private void Update()
        {
            if (Input.GetMouseButtonDown((int)this.MouseButton))
                this.SetNavMeshDestination();
        }

        private void SetNavMeshDestination()
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var mask = LayerMask.GetMask("Road");

            if (Physics.Raycast(ray, out hit, 1000f, mask))
            {
                var o = hit.transform;

                // Only set a waypoint if we click on a road.
                if (o.gameObject.layer == 10)
                    this.GetComponent<NavMeshAgent>().SetDestination(o.position);
            }
        }
    }
}