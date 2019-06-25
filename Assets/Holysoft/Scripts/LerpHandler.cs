using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft
{

    [System.Serializable]
    public class LerpHandler
    {
        public event EventAction<EventActionArgs> DestinationReach;

        [SerializeField]
        [MinValue(0)]
        private float m_speed;
        [SerializeField]
        private bool m_useLocal;

        private Transform m_transform;
        private Vector3 m_destination;
        private Vector3 m_startPosition;
        private float m_lerpValue;

        public void SetTransform(Transform transform)
        {
            m_transform = transform;
        }

        public void SetDestination(Vector3 destination)
        {
            m_startPosition = m_useLocal ? m_transform.localPosition : m_transform.position;
            m_destination = destination;
            m_lerpValue = 0;
        }

        public void Lerp(float deltaTime)
        {
            if (m_lerpValue != 1)
            {
                m_lerpValue = Mathf.Clamp(m_lerpValue + (m_speed * deltaTime), 0f, 1f);
                if (m_useLocal)
                {
                    m_transform.localPosition = Vector3.Lerp(m_startPosition, m_destination, m_lerpValue);
                }
                else
                {
                    m_transform.position = Vector3.Lerp(m_startPosition, m_destination, m_lerpValue);
                }
            }
        }
    }
}