using System.Collections;
using System.Collections.Generic;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    [AddComponentMenu("DChild/Gameplay/AI/Patrol/PlatformPatrolRemake")]
    public class PlatformPatrolRemake : PatrolHandle
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

        [SerializeField, TabGroup("Reference")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Reference")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("Reference")]
        private Transform objectToRotate;

        [SerializeField, BoxGroup("Configuration")]
        private int m_startIndex;
        [SerializeField, BoxGroup("Configuration")]
        private Iteration m_startIteration;
        [SerializeField, BoxGroup("Configuration")]
        private Transform[] m_cornerPoints;
        public Transform[] cornerPoints => m_cornerPoints;

        [BoxGroup("Direction to move")]
        [SerializeField]
        private bool m_clockwiseMovement;

        [SerializeField, MinValue(0)]
        private float m_nearDistanceTolerance = 0.1f;

        private int m_currentIndex;
        private Iteration m_iteration;
        private Vector2 m_currentTargetPosition;

        private void IterateToNextPoint()
        {
            if (m_currentIndex == m_cornerPoints.Length - 1)
            {
                m_iteration = Iteration.Backward;
            }
            else if (m_currentIndex == 0)
            {
                m_iteration = Iteration.Forward;
            }
            m_currentIndex += (int)m_iteration;
            m_currentTargetPosition = m_cornerPoints[m_currentIndex].position;
        }

        private bool IsNear(float position, float destination)
        {
            var distance = Mathf.Abs(destination - position);
            return distance <= m_nearDistanceTolerance;
        }

        public void Initialize()
        {
            m_currentIndex = m_startIndex;
            m_iteration = m_startIteration;
            IterateToNextPoint();
        }

        public void RotateMinion()
        {

        }

        public override void Patrol(MovementHandle2D movement, float speed, CharacterInfo character)
        {
            var currentPosition = character.position;
            var movementInfo = GetInfo(currentPosition);

            if(currentPosition != m_currentTargetPosition)
            {
                movement.MoveTowards(movementInfo.moveDirection, speed);
            }
            else
            {
                objectToRotate.position = m_currentTargetPosition;
                objectToRotate.rotation = Quaternion.Euler(0, 0, 90f);
            }
            
        }

        public override void Patrol(PathFinderAgent agent, float speed, CharacterInfo characterInfo)
        {
            throw new System.NotImplementedException();
        }

        public PatrolInfo GetInfo(Vector2 position)
        {
            var destination = m_cornerPoints[m_currentIndex];

            if (IsNear(position.x, destination.position.x) && IsNear(position.y, destination.position.y))
            {
                CallDestinationReached();
                IterateToNextPoint();
                destination = m_cornerPoints[m_currentIndex];
            }

            return new PatrolInfo(position, destination.position);
        }
    }
}

