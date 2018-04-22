using System.Collections;
using System.Collections.Generic;
using Assets.LD41.Scripts.Extensions;
using UnityEngine;

namespace Assets.LD41.Scripts
{
    public class RoadRenderer : MonoBehaviour
    {
        private List<GameObject> _roadObjects = new List<GameObject>();

        [SerializeField] private Material RoadMaterial;

        private void Awake()
        {
            RoadGen.OnGenerationComplete += this.Render;
        }

        private void Render(List<RoadData> roads)
        {
            this.Reset();

            foreach (var road in roads)
            {
                var g = this.RenderRoad(road);
                //g.transform.parent = this.transform;
                this._roadObjects.Add(g);
            }
        }

        private GameObject RenderRoad(RoadData road)
        {
            var slope = road.End.Slope(road.Start);
            var g = new GameObject(string.Format("Road (Slope:{0})", slope));
            var l = g.AddComponent<LineRenderer>();
            l.positionCount = 2;
            l.material = this.RoadMaterial;
            l.widthMultiplier = 3;
            l.SetPositions(new Vector3[]
            {
                    new Vector3(road.Start.x, 0f, road.Start.y),
                    new Vector3(road.End.x, 0f, road.End.y),
            });
            return g;
        }

        private void Reset()
        {
            foreach (var roadObject in this._roadObjects)
                GameObject.Destroy(roadObject);

            this._roadObjects.Clear();
        }
    }
}