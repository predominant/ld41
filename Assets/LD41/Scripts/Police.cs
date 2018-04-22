using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.LD41.Scripts.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.LD41.Scripts
{
    public enum PoliceState
    {
        None,
        Patrolling,
        Chasing,
        Eating,
        Resting,
    }

    public class Police : MonoBehaviour
    {
        public PoliceState CurrentState = PoliceState.Patrolling;
        [SerializeField] private PoliceState _lastState = PoliceState.None;
        [SerializeField] private float PatrolRange = 200f;
        [SerializeField] private LayerMask RoadLayer;
        [SerializeField] private float RestDuration = 7f;
        [SerializeField] private float LosePlayerTime = 5f;
        [SerializeField] private float NormalSpeed = 10f;
        [SerializeField] private float ChaseSpeed = 14f;

        [SerializeField] private Color PatrolColor;
        [SerializeField] private Color ChaseColor;
        [SerializeField] private Color RestColor;

        private NavMeshAgent _navAgent;
        private float _restStartTime = 0f;
        private float _restScheduleTime = 0f;
        private Camera _myCamera;
        private GameObject _player;
        private float _lastSeenPlayer = 0f;
        private Renderer _renderer;
        private AudioSource _sirenSound;

        private void Awake()
        {
            this._navAgent = this.GetComponent<NavMeshAgent>();
            this._myCamera = this.transform.Find("Camera").GetComponent<Camera>();
            this._player = GameObject.Find("PlayerCar");
            this._renderer = this.transform.Find("Car01").GetComponent<Renderer>();
            this._renderer.material.color = this.RestColor;
            this._sirenSound = this.GetComponent<AudioSource>();
        }

        private void Start()
        {
            this.StartCoroutine("CheckRestStatus");
            this.StartCoroutine("CheckPatrolStatus");
        }

        private void FixedUpdate()
        {
            this.CheckStateChange();
        }

        private void CheckStateChange()
        {
            if (this._lastState == this.CurrentState)
                return;

            this._lastState = this.CurrentState;

            this.StopAllCoroutines();

            switch (this.CurrentState)
            {
                case PoliceState.Patrolling:
                    this.Patrol();
                    this.StartCoroutine("CheckPatrolStatus");
                    break;
                case PoliceState.Resting:
                    this.StartCoroutine("CheckRestStatus");
                    this.Rest();
                    break;
                case PoliceState.None:
                    this.CurrentState = PoliceState.Patrolling;
                    break;
                case PoliceState.Chasing:
                    this.StartCoroutine("CheckChaseStatus");
                    this.Chase();
                    break;
            }
        }

        private void Patrol()
        {
            this._renderer.material.color = this.PatrolColor;
            this._navAgent.isStopped = false;
            this._navAgent.speed = this.NormalSpeed;

            var tiles1 = Physics.OverlapSphere(this.transform.position, this.PatrolRange, this.RoadLayer.value);
            var tiles2 = Physics.OverlapSphere(this.transform.position, this.PatrolRange - 20f, this.RoadLayer.value);
            var tiles = new List<Transform>();

            foreach (var tile in tiles1)
            {
                if (!tiles2.Contains(tile))
                    tiles.Add(tile.transform);
            }

            var target = tiles[Random.Range(0, tiles.Count)].position;
            //Debug.Log("Setting Destination for Patrol");
            this._navAgent.SetDestination(target);
        }

        private IEnumerator CheckPatrolStatus()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.5f);

                if (this.CurrentState != PoliceState.Patrolling)
                    break;

                if (this._myCamera.IsVisibleFrom(this._player.transform.GetChild(0).GetComponent<Renderer>()))
                {
                    //Debug.Log(string.Format("{0}: I can see the player", this.gameObject.name));
                    this._lastSeenPlayer = Time.time;
                    this._renderer.material.color = this.ChaseColor;
                    this._sirenSound.Play();
                    this.CurrentState = PoliceState.Chasing;
                    continue;
                }

                if (this._navAgent.remainingDistance <= 1f)
                {
                    //Debug.Log("Patrol Complete - Setting state to Resting");
                    this.CurrentState = PoliceState.Resting;
                    continue;
                }
            }
        }

        private void Rest()
        {
            this._renderer.material.color = this.RestColor;
            this._navAgent.speed = this.NormalSpeed;
            this._navAgent.isStopped = true;
            this._restStartTime = Time.time;
            this._restScheduleTime = Random.Range(this.RestDuration / 2f, this.RestDuration + this.RestDuration / 2f);
        }

        private IEnumerator CheckRestStatus()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(this._restScheduleTime / 3f);

                if (this.CurrentState != PoliceState.Resting)
                    break;

                if (Time.time - this._restStartTime > this._restScheduleTime)
                {
                    //Debug.Log("Rest Complete - Starting a new patrol");
                    this.CurrentState = PoliceState.Patrolling;
                    continue;
                }
            }
        }

        private void Chase()
        {
            this._navAgent.speed = this.ChaseSpeed;
            this._navAgent.SetDestination(this._player.transform.position);
        }

        private IEnumerator CheckChaseStatus()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.1f);

                if (this.CurrentState != PoliceState.Chasing)
                    break;

                while (true)
                {
                    if (this._myCamera.IsVisibleFrom(this._player.transform.GetChild(0).GetComponent<Renderer>()))
                    {
                        //Debug.Log(string.Format("{0}: I can see the player", this.gameObject.name));
                        this._lastSeenPlayer = Time.time;
                    }

                    if (Time.time - this._lastSeenPlayer > this.LosePlayerTime)
                    {
                        //Debug.Log("Lost the suspect - Taking a break");
                        this._sirenSound.Stop();
                        this.CurrentState = PoliceState.Resting;
                        break;
                    }

                    this.Chase();
                    yield return new WaitForSecondsRealtime(this.LosePlayerTime / 10f);
                }
            }
        }

        private void OnDrawGizmos()
        {
            //Gizmos.DrawWireSphere(this.transform.position, this.PatrolRange);
            //Gizmos.DrawWireSphere(this.transform.position, this.PatrolRange - 20f);

            if (this._navAgent != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(this._navAgent.destination, 10f);

                if (this._navAgent.hasPath)
                {
                    for (var i = 0; i < this._navAgent.path.corners.Length - 1; i++)
                    {
                        Debug.DrawLine(this._navAgent.path.corners[i], this._navAgent.path.corners[i + 1], Color.red);
                    }
                }
            }
        }
    }
}