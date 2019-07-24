using DChild.Gameplay.Characters;
using UnityEngine;

namespace DChild.Gameplay
{
    [RequireComponent(typeof(RaySensor))]
    public class RaySensorFaceRotator : MonoBehaviour, ISensorFaceRotation
    {
        [SerializeField]
        [Tooltip("Uses Local Rotation")]
        private float m_leftRotation;
        [SerializeField]
        [Tooltip("Uses Local Rotation")]
        private float m_rightRotation;

        private RaySensor m_sensor;

        public void AlignRotationToFacing(HorizontalDirection direction)
        {
#if UNITY_EDITOR
            if(m_sensor == null)
            {
                m_sensor = GetComponent<RaySensor>();
            }
#endif
            m_sensor.SetRotation(direction == HorizontalDirection.Left ? m_leftRotation : m_rightRotation);
        }

        private void Awake()
        {
            m_sensor = GetComponent<RaySensor>();
        }

#if UNITY_EDITOR
        public void SetRotations(float leftRotation, float rightRotation)
        {
            m_leftRotation = leftRotation;
            m_rightRotation = rightRotation;
        }
#endif 
    }
}