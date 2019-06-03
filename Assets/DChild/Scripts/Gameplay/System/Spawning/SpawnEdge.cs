using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    public class SpawnEdge : SpawnArea
    {
        [SerializeField]
        [ListDrawerSettings(CustomAddFunction = "CreateNewEdgePoint")]
        [RequiredVectors2(2)]
        private Vector2[] m_edgePoints;

        public override Vector2 GetRandomPosition()
        {
            var edgePoint = (uint)Random.Range(0, m_edgePoints.Length - 1);
            return GetPositionFromEdgePoint(edgePoint);
        }

        private Vector2 GetPositionFromEdgePoint(uint pointIndex)
        {
            var position = (Vector2)transform.position;
            var currentPoint = position + m_edgePoints[pointIndex];
            var nextPoint = position + m_edgePoints[pointIndex + 1];

            var difference = nextPoint - currentPoint;
            var offset = Random.Range(0f, difference.magnitude);
            return currentPoint + (difference.normalized * offset);
        }

#if UNITY_EDITOR
        public Vector2[] edgePoints
        {
            get
            {
                return m_edgePoints;
            }

            set
            {
                m_edgePoints = value;
            }
        }

        private Vector2 CreateNewEdgePoint()
        {
            var currentLength = m_edgePoints.Length;
            var lastPoint = m_edgePoints[currentLength - 1];
            var prevLastPoint = m_edgePoints[currentLength - 2];

            var flowDirection = (lastPoint - prevLastPoint).normalized;
            return lastPoint + (flowDirection * 2f);
        }
#endif
    }
}