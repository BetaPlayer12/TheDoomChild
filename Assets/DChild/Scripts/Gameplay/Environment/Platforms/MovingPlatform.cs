﻿using DChild.Gameplay.Systems.WorldComponents;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [AddComponentMenu("DChild/Gameplay/Environment/Moving Platform")]
    public class MovingPlatform : MonoBehaviour, ISerializableComponent
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

        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private Vector2 m_position;
            [SerializeField]
            private int m_wayPoint;
            [SerializeField]
            private int m_incrementerValue;

            public SaveData(Vector2 position, int wayPoint, int incrementerValue)
            {
                m_position = position;
                m_wayPoint = wayPoint;
                m_incrementerValue = incrementerValue;
            }

            public Vector2 position => m_position;
            public int wayPoint => m_wayPoint;
            public int incrementerValue => m_incrementerValue;
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

        private int m_pingPongWaypoint;

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

        public ISaveData Save() => new SaveData(m_cacheDestination, m_wayPointDestination, m_incrementerValue);

        public void Load(ISaveData data)
        {
            var saveData = (SaveData)data;
            transform.position = saveData.position;
            m_cacheDestination = saveData.position;
            m_cacheCurrentWaypoint = m_cacheDestination;
            m_wayPointDestination = saveData.wayPoint;
            m_currentWayPoint = m_wayPointDestination;
            m_incrementerValue = saveData.incrementerValue;
        }

        public void PingPongNextWaypoint(bool next)
        {
            m_pingPongWaypoint += next ? 1 : -1;
            m_wayPointDestination = (int)Mathf.PingPong(m_pingPongWaypoint, m_listSize - 1);
            ChangeDestination();
        }

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
            m_pingPongWaypoint = destination;
            m_wayPointDestination = destination;
            ChangeDestination();
        }

        public void TeleportTo(int destination)
        {
            m_pingPongWaypoint = destination;
            m_wayPointDestination = destination;
            ChangeDestination();
            enabled = false;
            transform.position = m_cacheDestination;
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

                int proposedIncrementerValue = m_currentWayPoint > m_wayPointDestination ? -1 : 1;

                if (proposedIncrementerValue != m_incrementerValue)
                {
                    m_incrementerValue = proposedIncrementerValue;
                    m_currentWayPoint += m_incrementerValue;
                    m_cacheCurrentWaypoint = m_waypoints[m_currentWayPoint];
                }
                // Maintain Pathway to Destination
                else if (enabled == false)
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
            var currentPosition = (Vector2)transform.position;
            if (currentPosition != m_cacheDestination)
            {
                transform.position = Vector2.MoveTowards(currentPosition, m_cacheCurrentWaypoint, m_speed * m_isolatedTime.deltaTime);
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

            if (GetComponent<IsolatedObject>() == null)
            {
                gameObject.AddComponent<IsolatedObject>();
            }
        }
    }
}




