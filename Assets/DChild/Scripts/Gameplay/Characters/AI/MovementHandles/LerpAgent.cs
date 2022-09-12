using Pathfinding;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay
{
    [AddComponentMenu("DChild/Gameplay/AI/Movement/Lerp Agent")]
    public class LerpAgent : PathFinderAgent
    {
        [SerializeField]
        private AILerp m_ai;
        [SerializeField]
        private Transform m_positionReference;

        private Vector2 m_destination;

        public override Vector2 segmentDestination => /*m_ai.interpolator.GetSegment(m_ai.interpolator.segmentIndex + 1)*/ m_positionReference.position + m_ai.interpolator.tangent.normalized;

        public override bool hasPath => m_ai.hasPath;

        public override void Move(float speed)
        {
            m_ai.speed = speed;
            if (m_ai.canMove == false)
            {
                ResetAILerp();
            }
        }

        public override void MoveTowardsForced(Vector2 direction, float speed)
        {
            m_ai.destination = m_positionReference.position + (Vector3)direction;
            m_ai.speed = speed;
            if (m_ai.canMove == false)
            {
                ResetAILerp();
            }

        }

        private void ResetAILerp()
        {
            m_ai.enabled = false;
            m_ai.enabled = true;
            m_ai.canMove = true;
        }

        public override void SetDestination(Vector2 position)
        {
            m_destination = position;
        }

        public override void Stop()
        {
            m_ai.canMove = false;
        }

        private void Update()
        {
            m_ai.destination = m_destination;
        }

        private void OnEnable()
        {
            m_ai.onSearchPath += Update;
        }

        private void OnDisable()
        {
            m_ai.onSearchPath -= Update;
        }
    }
}