using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [AddComponentMenu("DChild/Gameplay/Environment/Moving Platform")]
    public class MovingPlatform : MonoBehaviour
    {
        public struct UpdateEventArgs : IEventActionArgs
        {
            public UpdateEventArgs(int instance, int currentWaypointIndex, int waypointCount, bool isGoingForward) : this()
            {
                this.instance = instance;
                this.currentWaypointIndex = currentWaypointIndex;
                this.waypointCount = waypointCount;
                this.isGoingForward = isGoingForward;
            }

            public int instance { get; }
            public int currentWaypointIndex { get; }
            public int waypointCount { get; }
            public bool isGoingForward { get; }
        }

        [SerializeField, MinValue(0.1f), TabGroup("Setting")]
        private float m_speed;
        [SerializeField, OnValueChanged("ValidateStartingWaypoint"), TabGroup("Setting")]
        private int m_startWaypoint;
        [SerializeField, ListDrawerSettings(CustomAddFunction = "AddWaypoint"), TabGroup("Setting"), HideInInlineEditors]
        private Vector2[] m_waypoints;
        [ShowInInspector, OnValueChanged("ChangeDestination"), TabGroup("Debug")]
        private int m_wayPointDestination;
        [ShowInInspector, ReadOnly, TabGroup("Debug")]
        private int m_currentWayPoint;
        private int m_incrementerValue;

        private Rigidbody2D m_rigidbody;
        private IIsolatedTime m_isolatedTime;
        private Vector2 m_cacheDestination;
        private Vector2 m_cacheCurrentWaypoint;
        private int m_listSize;

        public event EventAction<UpdateEventArgs> DestinationReached;

#if UNITY_EDITOR
        public Vector2[] waypoints { get => m_waypoints; set => m_waypoints = value; }

        private Vector2 AddWaypoint() => transform.position;

        private void ValidateStartingWaypoint()
        {
            m_startWaypoint = (int)Mathf.Repeat(m_startWaypoint, m_waypoints.Length);
            transform.position = m_waypoints[m_startWaypoint];
        }
#endif

        public void GoToNextWayPoint()
        {
            if (m_currentWayPoint < m_listSize - 1)
            {
                m_wayPointDestination++;
                ChangeDestination();
            }
        }

        public void GoToPreviousWaypoint()
        {
            if (m_currentWayPoint > 0)
            {
                m_wayPointDestination--;
                ChangeDestination();
            }
        }

        public void GoDestination(int destination)
        {
            m_wayPointDestination = destination;
            ChangeDestination();
        }

        public void Initialize(int startingIndex, int destination)
        {
            m_currentWayPoint = startingIndex;
            transform.position = m_waypoints[m_currentWayPoint];
            GoDestination(destination);
        }

        private void ChangeDestination()
        {
            if (m_currentWayPoint != m_wayPointDestination)
            {

                int proposedIncrementerValue = 0;
                if (m_currentWayPoint > m_wayPointDestination)
                {
                    proposedIncrementerValue = -1;
                }
                else
                {
                    proposedIncrementerValue = 1;
                }

                if (proposedIncrementerValue != m_incrementerValue)
                {
                    m_incrementerValue = proposedIncrementerValue;
                }

                if (enabled == false)
                {
                    m_currentWayPoint += m_incrementerValue;
                    m_cacheCurrentWaypoint = m_waypoints[m_currentWayPoint];
                }
                m_cacheDestination = m_waypoints[m_wayPointDestination];
            }

            enabled = true;
        }

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_rigidbody.position = m_waypoints[m_startWaypoint];
            m_isolatedTime = GetComponent<IIsolatedTime>();
            m_wayPointDestination = m_startWaypoint;
            m_currentWayPoint = m_wayPointDestination;
            m_cacheCurrentWaypoint = m_waypoints[m_currentWayPoint];
            m_listSize = m_waypoints.Length;
            ChangeDestination();
        }

        private void Update()
        {
            var currentPosition = m_rigidbody.position;
            if (currentPosition != m_cacheDestination)
            {
                m_rigidbody.position = Vector2.MoveTowards(currentPosition, m_cacheCurrentWaypoint, m_speed * m_isolatedTime.deltaTime);
                if (currentPosition == m_cacheCurrentWaypoint)
                {
                    m_currentWayPoint += m_incrementerValue;
                    m_cacheCurrentWaypoint = m_waypoints[m_currentWayPoint];
                }
            }
            else
            {
                enabled = false;
                DestinationReached?.Invoke(this, new UpdateEventArgs(GetInstanceID(), m_currentWayPoint, m_listSize, m_incrementerValue == 1));
            }
        }

        private void OnValidate()
        {
            if (GetComponent<Rigidbody2D>() == null)
            {
                var rigidbody = gameObject.AddComponent<Rigidbody2D>();
                rigidbody.isKinematic = true;
            }

            if(GetComponent<IsolatedObject>() == null)
            {
                gameObject.AddComponent<IsolatedObject>();
            }
        }
    }
}




