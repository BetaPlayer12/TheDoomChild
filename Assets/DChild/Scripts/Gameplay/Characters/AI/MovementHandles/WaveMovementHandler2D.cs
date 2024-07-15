using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{

    public class WaveMovementHandler2D : MovementHandle2D
    {
        [SerializeField]
        protected Rigidbody2D m_source;
        [SerializeField, MinValue(0)]
        private float m_verticalAmplitude;
        [SerializeField, MinValue(0)]
        private float m_waveSpeed;
        [SerializeField]
        private bool m_useCosine;

        private Vector3 m_startingPostion;
        private float m_sineTime;

        public override void MoveTowards(Vector2 direction, float speed)
        {
            var position = m_source.position;
            var normalizedDirection = direction.normalized;
            var deltaTime = GameplaySystem.time.deltaTime;
            if (deltaTime > 0)
            {
                m_sineTime += deltaTime;
                position += normalizedDirection * speed * deltaTime;
                var waveValue = 0f;
                if (m_useCosine)
                {
                    waveValue = (Mathf.Cos(m_sineTime * m_waveSpeed));
                }
                else
                {
                    waveValue = (Mathf.Sin(m_sineTime * m_waveSpeed));
                }
                var perpendicular = new Vector2(normalizedDirection.y, -normalizedDirection.x);
                position += perpendicular * (waveValue * m_verticalAmplitude);
                m_source.position = position;
            }
        }

        public override void Stop()
        {
            m_startingPostion = m_source.position;
        }

        private void Awake()
        {
            m_startingPostion = m_source.position;
            m_sineTime = 0;
        }
    }
}