using System.Collections;
using System.Collections.Generic;
using Assets.LD41.Scripts.Extensions;
using UnityEngine;

namespace Assets.LD41.Scripts
{
    public struct RoadData
    {
        public Vector2 Start;
        public Vector2 End;
    }

    [RequireComponent(typeof(RoadRenderer))]
    public class RoadGen : MonoBehaviour
    {
        public delegate void GenerationComplete(List<RoadData> roads);

        public static event GenerationComplete OnGenerationComplete;

        [SerializeField] private int RoadCount = 10;
        [SerializeField] private float Scale = 100;

        private List<RoadData> Roads = new List<RoadData>();

        private void Awake()
        {
            //Random.InitState(0);
        }

        private void Start()
        {
            this.GenerateRoads();
        }

        private void OnValidate()
        {
            //this.GenerateRoads();
        }

        private void GenerateRoads()
        {
            this.Roads.Clear();
            var spacing = 5f;

            for (var i = 0; i < this.RoadCount; i++)
            {
                var regen = true;
                var road = new RoadData();
                while (regen)
                {
                    regen = false;
                    road = this.GenerateRoad();
                    foreach (var r in this.Roads)
                    {
                        if (Mathf.Abs(road.Start.x - r.Start.x) < spacing
                            || Mathf.Abs(road.Start.y - r.Start.y) < spacing
                            || Mathf.Abs(road.End.x - r.End.x) < spacing
                            || Mathf.Abs(road.End.y - r.End.y) < spacing)
                        {
                            regen = true;
                            break;
                        }
                    }
                }
                this.Roads.Add(road);
            }

            if (OnGenerationComplete != null)
                OnGenerationComplete(this.Roads);
        }

        private RoadData GenerateRoad()
        {
            var slope = 1f;

            var start = Random.insideUnitCircle * this.Scale;
            var end = new Vector2();

            while (slope > 0.1f && slope < 5f)
            {
                end = Random.insideUnitCircle * this.Scale;
                slope = end.Slope(start);
            }

            return new RoadData {Start = start, End = end};
        }
    }
}