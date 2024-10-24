﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    [System.Serializable]
    public sealed class WayPointPatroler : MonoBehaviour
    {
        public struct PatrolInfo
        {
            private Vector2 m_destination;
            private Vector2 m_moveDirection;

            public PatrolInfo(Vector2 currentPosition, Vector2 destination)
            {
                m_destination = destination;
                m_moveDirection = (destination - currentPosition).normalized;
            }

            public Vector2 destination => m_destination;
            public Vector2 moveDirection => m_moveDirection;
        }

        public enum Iteration
        {
            Forward = 1,
            Backward = -1
        }

        public bool isNearDestination;

        [BoxGroup("Significant Axis")]
        [SerializeField]
        private bool m_useX = true;
        [BoxGroup("Significant Axis")]
        [SerializeField]
        private bool m_useY;
        [SerializeField]
        [MinValue(0)]
        [Tooltip("Max Distance to consider object is near")]
        private float m_nearDistanceTolerance = 0.1f;

        [SerializeField]
        [OnValueChanged("UpdateStartIndex")]
        [BoxGroup("Configuration")]
        private int m_startIndex;
        [SerializeField]
        [BoxGroup("Configuration")]
        private Iteration m_startIteration;
        [SerializeField]
        [BoxGroup("Configuration")]
        [ListDrawerSettings(CustomAddFunction = "AddToWaypoint")]
        private Vector2[] m_wayPoints;

        private int m_currentIndex;
        private Iteration m_iteration;

        public void Initialize()
        {
            m_currentIndex = m_startIndex;
            m_iteration = m_startIteration;
            NextPatrolPoint();
        }

        public PatrolInfo GetInfo(Vector2 position)
        {
            var destination = m_wayPoints[m_currentIndex];
            bool nearX = m_useX ? IsNear(position.x, destination.x) : true;
            bool nearY = m_useY ? IsNear(position.y, destination.y) : true;

            if (nearX && nearY)
            {
                //Debug.Log("Switch Destinations");
                isNearDestination = true;
                NextPatrolPoint();
                destination = m_wayPoints[m_currentIndex];
            }
            else
            {
                isNearDestination = false;
            }

            return new PatrolInfo(position, destination);
        }

        private bool IsNear(float position, float destination)
        {
            var distance = Mathf.Abs(destination - position);
            return distance <= m_nearDistanceTolerance;
        }

        private void NextPatrolPoint()
        {
            if (m_currentIndex == m_wayPoints.Length - 1)
            {
                m_iteration = Iteration.Backward;
            }
            else if (m_currentIndex == 0)
            {
                m_iteration = Iteration.Forward;
            }
            m_currentIndex += (int)m_iteration;
        }

#if UNITY_EDITOR
        [Space]

        [FoldoutGroup("ToolKit")]
        [SerializeField]
        private bool m_useCurrentPosition;
        [FoldoutGroup("ToolKit")]
        [SerializeField]
        [MinValue(0), OnValueChanged("UpdateStartIndex")]
        private int m_overridePatrolIndex;

        public bool useCurrentPosition => m_useCurrentPosition;
        public int overridePatrolIndex => m_overridePatrolIndex;
        public int iteration => (int)m_startIteration;
        public int startIndex => m_startIndex;
        public Vector2[] wayPoints => m_wayPoints;

        [FoldoutGroup("ToolKit")]
        [HideIf("m_useCurrentPosition")]
        [Button("Save To Current Index")]
        private void SaveToCurrentIndex()
        {
            m_wayPoints[m_overridePatrolIndex] = transform.position;
        }

        [FoldoutGroup("ToolKit")]
        [Button("Go To Starting Position")]
        private void GoToStartingPosition()
        {
            m_useCurrentPosition = false;
            transform.position = m_wayPoints[m_startIndex];
        }

        private Vector2 AddToWaypoint() => transform.position;

        private void UpdateStartIndex()
        {
            if (m_wayPoints != null)
            {
                m_startIndex = Mathf.Min(m_startIndex, m_wayPoints.Length - 1);

                m_overridePatrolIndex = Mathf.Min(m_overridePatrolIndex, m_wayPoints.Length - 1);

            }
        }
#endif
    }
}