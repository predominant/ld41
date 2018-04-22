using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.LD41.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class RenderNavPath : MonoBehaviour
    {
        private bool hasPath = false;
        private NavMeshAgent _navAgent;
        private LineRenderer _lineRenderer;

        [SerializeField] private Material LineMaterial;

        private void Awake()
        {
            this._navAgent = this.GetComponent<NavMeshAgent>();
            this._lineRenderer = this.gameObject.AddComponent<LineRenderer>();
            this._lineRenderer.material = this.LineMaterial;
            this._lineRenderer.textureMode = LineTextureMode.Tile;
        }

        private void Update()
        {
            if (this._navAgent.hasPath)
            {
                this.hasPath = true;
                this.RenderPath();
            }
        }

        private void RenderPath()
        {
            var path = this._navAgent.path;
            this._lineRenderer.numCornerVertices = 5;
            this._lineRenderer.positionCount = path.corners.Length;
            this._lineRenderer.SetPositions(path.corners.Reverse().ToArray());
        }
    }
}