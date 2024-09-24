using Holysoft.Event;
using Pathfinding;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Pathfinding
{
    public class NavigationTracker : MonoBehaviour
    {
        public event EventAction<EventActionArgs> DestinationReached; 

        [InfoBox("Make Sure there is a SEEKER component in parent gameObject")]
        [SerializeField, MinValue(0.1f)]
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
        private bool m_pathUpdated;
        private bool m_isRefreshing;
        private Vector3 m_currentPathSegment;
        private Vector3 m_previousPathSegment;
        private Vector3 m_lastPathSegment;

        private bool m_wasEntityInStartingPathSegment;
        private bool m_canSearchPath;

        public bool refreshPath
        {
            set
            {
                m_refreshPath = value;
            }
        }

        public bool pathError => m_lastPathHasError;
        public bool pathUpdated => m_pathUpdated;
        public Vector3 currentPathSegment => m_currentPathSegment;
        public Vector3 previousPathSegment => m_previousPathSegment;
        public Vector3 lastPathSegment => m_lastPathSegment;
        public bool isOnLastPathSegment => m_pathIndex == m_path.Count;
        public Vector3 directionToPathSegment => (m_currentPathSegment - transform.position).normalized;

        public bool wasEntityInStartingPathSegment => m_wasEntityInStartingPathSegment;

        private Vector3 m_prevDirectionToPathSegment;

        public bool IsCurrentDestination(Vector3 destination)
        {
            return m_destination == destination;
        }

        public void SetDestination(Vector3 destination)
        {
            SetDestination(transform.position, destination);
        }

        public void SetDestination(Vector3 fromPosition, Vector3 destination)
        {
            if (destination == transform.position)
            {
                m_destination = destination;
            }
            else
            {
                enabled = true;
                if (m_destination == destination)
                {
                    return;
                }

                m_destination = destination;
                if (m_canSearchPath)
                {
                    SearchPathTo(m_destination);
                    m_pathUpdated = false;
                }
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
            if (enabled && m_canSearchPath)
            {
                SearchPathTo(m_destination);
            }
        }

        private void RefreshPath(float deltaTime)
        {
            m_refreshTimer -= deltaTime;
            if (m_refreshTimer <= 0f && m_canSearchPath)
            {
                SearchPathTo(m_destination);
                m_pathUpdated = false;
                m_isRefreshing = true;
            }
        }

        private void SearchPathTo(Vector3 destination)
        {
            m_seeker.CancelCurrentPathRequest();
            m_seeker.StartPath(transform.position, destination);
            m_canSearchPath = false;
        }

        private void OnSetPathReturn(Path p)
        {
            if (p.error)
            {
                m_path = null;
                m_refreshTimer = 0;
                m_lastPathHasError = true;
                m_currentPathSegment = Vector3.zero;
                m_previousPathSegment = Vector3.zero;
                m_lastPathSegment = Vector3.zero;
                m_wasEntityInStartingPathSegment = false;
            }
            else
            {
                m_path = p.vectorPath;
                m_pathIndex = 1;
                m_refreshTimer = m_refreshRate;
                m_lastPathHasError = false;
                m_currentPathSegment = m_path[1];
                m_previousPathSegment = m_path[0];
                m_lastPathSegment = m_path[m_path.Count - 1];
                m_prevDirectionToPathSegment = directionToPathSegment;
                m_pathUpdated = true;

                m_wasEntityInStartingPathSegment = transform.position == m_path[0];
            }
            m_canSearchPath = true;
        }

        private void TrackPath()
        {
            bool hasReachedDestination = Vector3.Distance(transform.position, m_currentPathSegment) <= m_destinationTolerance;
            if (hasReachedDestination == false)
            {
                hasReachedDestination = (m_prevDirectionToPathSegment + directionToPathSegment == Vector3.zero);
            }

            m_prevDirectionToPathSegment = directionToPathSegment;
            if (hasReachedDestination)
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
                    m_previousPathSegment = m_currentPathSegment;
                    m_currentPathSegment = m_path[m_pathIndex];
                    m_prevDirectionToPathSegment = directionToPathSegment;
                }
            }
        }

        private void Awake()
        {
            m_seeker = GetComponentInParent<Seeker>();
            m_seeker.startEndModifier.adjustStartPoint = () => transform.position;
            m_seeker.pathCallback += OnSetPathReturn;
            m_canSearchPath = true;
        }

        private void Start()
        {
            enabled = true;
        }

        private void Update()
        {
            if (m_lastPathHasError)
            {
                m_seeker.StartPath(transform.position, m_destination);
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

