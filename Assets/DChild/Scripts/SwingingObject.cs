using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    public class SwingingObject : MonoBehaviour
    {
        [SerializeField]
        private float m_maxSwingAngle = 1.5f;
        [SerializeField, MinValue(0)]
        private float m_speed = 2f;
        [SerializeField]
        private bool m_useCosine;
        [SerializeField]
        private float m_timeOffset;

        private Vector3 m_startRotation;

        private void Start()
        {
            m_startRotation = transform.rotation.eulerAngles;
        }

        private void LateUpdate()
        {
            var nextRotation = m_startRotation;
            if (m_useCosine)
            {
                nextRotation.z += m_maxSwingAngle * Mathf.Cos((Time.time + m_timeOffset) * m_speed);
            }
            else
            {
                nextRotation.z += m_maxSwingAngle * Mathf.Sin((Time.time + m_timeOffset) * m_speed);
            }
            transform.rotation = Quaternion.Euler(nextRotation);
        }
    }
}