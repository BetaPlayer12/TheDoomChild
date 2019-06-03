using Holysoft.Event;
using Pathfinding;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Pathfinding
{
    [RequireComponent(typeof(Seeker))]
    public class NavigationTracker : MonoBehaviour
    {
        public event EventAction<EventActionArgs> DestinationReached;

        [SerializeField]
        [MinValue(0.1f)]
        private float m_destinationTolerance;
        [SerializeField]
        private bool m_refreshPath;
        [SerializeField]
        [MinValue(0f)]
        [ShowIf("m_refreshPath")]
        private float m_refreshRate;

        private Seeker m_seeker;
        private float m_refreshTimer;
        private Vector3 m_destination;
        private List<Vector3> m_path;
        private int m_pathIndex;
        private bool m_lastPathHasError;
        private bool m_isRefreshing;
        private Vector3 m_currentPathSegment;
        private Vector3 m_lastPathSegment;

        public bool refreshPath
        {
            set
            {
                m_refreshPath = value;
            }
        }

        public bool pathError => m_lastPathHasError;
        public Vector3 currentPathSegment => m_currentPathSegment;
        public Vector3 lastPathSegment => m_lastPathSegment;
        public bool isOnLastPathSegment => m_pathIndex == m_path.Count;
        public Vector3 directionToPathSegment => (m_currentPathSegment - transform.position).normalized;

        public bool IsCurrentDestination(Vector3 destination)
        {
            return m_destination == destination;
        }

        public void SetDestination(Vector3 destination)
        {
            if (destination == transform.position)
            {
                m_destination = destination;
            }
            else
            {
                enabled = true;
                if (destination == m_destination)
                {
                    return;
                }

                m_destination = destination;
                m_seeker.StartPath(transform.position, m_destination, OnSetPathReturn);
                Debug.Log("Seek");
            }
        }

        public void StopTracking()
        {
            m_path = null;
            m_lastPathHasError = false;
            m_isRefreshing = false;
            enabled = false;
        }

        public void RecalcuatePath()
        {
            if (enabled)
            {
                m_seeker.StartPath(transform.position, m_destination, OnSetPathReturn);
            }
        }

        private void RefreshPath(float deltaTime)
        {
            m_refreshTimer -= deltaTime;
            if (m_refreshTimer <= 0f)
            {
                var path = m_seeker.StartPath(transform.position, m_destination, OnSetPathReturn);
                m_isRefreshing = true;
            }
        }

        private void UseMostRelevantSegment()
        {
            var previousDistance = Vector3.Distance(m_path[0], transform.position);
            for (int i = 1; i < m_path.Count; i++)
            {
                var currentDistance = Vector3.Distance(m_path[i], transform.position);
                if (previousDistance >= currentDistance)
                {
                    previousDistance = currentDistance;
                }
                else
                {
                    m_pathIndex = i;
                    m_currentPathSegment = m_path[i];
                    break;
                }
            }
        }

        private void OnSetPathReturn(Path p)
        {
            if (p.error)
            {
                m_path = null;
                m_refreshTimer = 0;
                m_lastPathHasError = true;
                m_currentPathSegment = Vector3.zero;
                m_lastPathSegment = Vector3.zero;
            }
            else
            {
                m_path = p.vectorPath;
                m_pathIndex = 1;
                m_refreshTimer = m_refreshRate;
                m_lastPathHasError = false;
                m_currentPathSegment = m_path[1];
                m_lastPathSegment = m_path[m_path.Count - 1];
            }
        }

        private void TrackPath()
        {
            if (Vector3.Distance(transform.position, m_currentPathSegment) <= m_destinationTolerance)
            {
                if (m_pathIndex == m_path.Count - 1)
                {
                    //Destination Reached;
                    DestinationReached?.Invoke(this, EventActionArgs.Empty);
                    enabled = false;
                }
                else
                {
                    m_pathIndex++;
                    m_currentPathSegment = m_path[m_pathIndex];
                }
            }
        }

        private void Awake() => m_seeker = GetComponent<Seeker>();

        //private void OnEnable() => NavigationManager.Register(this);

        //private void OnDisable() => NavigationManager.Unregister(this);

        private void Update()
        {
            if (m_lastPathHasError)
            {
                m_seeker.StartPath(transform.position, m_destination, OnSetPathReturn);
            }
            else if (m_refreshPath)
            {
                RefreshPath(Time.deltaTime);
            }

            TrackPath();
        }

        private void OnValidate()
        {
            enabled = false;
        }
    }
}

