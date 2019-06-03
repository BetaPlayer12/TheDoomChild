/*********************************
 * 
 * This Platform Changes its position when called;
 * 
 ********************************/

using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment.Platforms
{
    public class RepositionPlatform : MonoBehaviour
    {
        [SerializeField]
        [MinValue(0.1f)]
        private float m_travelTime;
        [SerializeField]
        [RequiredVectors2(2)]
        [ListDrawerSettings(CustomAddFunction = "CreatePosition")]
        private Vector2[] m_positions;

        private ParticleFX m_movingFX;
        private bool m_isMoving;
        private Vector2 m_destination;
        private Vector2 m_initialPosition;
        private float m_lerpTime;
        private float m_lerpModifier;

        public int positionCount => m_positions.Length;

        public void MoveTo(uint positionIndex)
        {
            m_isMoving = true;
            m_initialPosition = transform.position;
            m_destination = m_positions[positionIndex];
            m_lerpTime = 0f;
            m_movingFX?.Play();
        }

        public void SnapTo(uint positionIndex)
        {
            transform.position = m_positions[positionIndex];
            m_isMoving = false;
            m_movingFX?.Stop();
        }

        private void Awake()
        {
            m_lerpModifier = 1f / m_travelTime;
            m_movingFX = GetComponentInChildren<ParticleFX>();
        }

        // Use this for initialization
        private void Update()
        {
            if (m_isMoving)
            {
                if (m_lerpTime >= 1f)
                {
                    m_lerpTime = 1f;
                    m_isMoving = false;
                    m_movingFX?.Stop();
                }
                transform.position = Vector2.Lerp(m_initialPosition, m_destination, m_lerpTime);
                m_lerpTime += m_lerpModifier * GameplaySystem.time.deltaTime;
            }
        }

        private void OnValidate()
        {
            m_lerpModifier = 1f / m_travelTime;
        }

#if UNITY_EDITOR
        public Vector2[] positions
        {
            get
            {
                return m_positions;
            }

            set
            {
                m_positions = value;
            }
        }

        private Vector2 CreatePosition() => transform.position;

        private int m_index = 0;
        [Button("Next Move")]
        private void NextMove()
        {
            SnapTo((uint)m_index);
            m_index = (int)Mathf.Repeat(m_index + 1, m_positions.Length);
            MoveTo((uint)m_index);
        }

        [Button("Prev Move")]
        private void PrevMove()
        {
            SnapTo((uint)m_index);
            m_index = (int)Mathf.Repeat(m_index - 1, m_positions.Length);
            MoveTo((uint)m_index);
        }
#endif
    }

}