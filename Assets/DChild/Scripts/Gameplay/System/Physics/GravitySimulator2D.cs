using Holysoft;
using DChild.Gameplay.Systems.WorldComponents;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Physics
{
    [System.Serializable]
    public class GravitySimulator2D : IGravity2D
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_gravityScale = 1f;

        private Vector2 m_direction;
        private Rigidbody2D m_rigidbody;
        private IIsolatedTime m_time;

        private const float GRAVITYFORCE = 9.87f;

        public float gravityScale
        {
            get
            {
                return m_gravityScale;
            }
            set
            {
                m_gravityScale = value;
            }
        }

        public void Initialize(Rigidbody2D rigidbody, IIsolatedTime isolatedTime )
        {
            m_rigidbody = rigidbody;
            m_time = isolatedTime;
        }

        public void SetAngle(Vector2 direction)
        {
            m_direction = direction;
#if UNITY_EDITOR
            m_angle = Vector2.Angle(Vector2.right, direction);
#endif
        }

        public void AddGravity(float scale) => m_rigidbody.velocity += m_direction * Mathf.Abs(GRAVITYFORCE) * m_gravityScale / m_rigidbody.mass * (m_time?.fixedDeltaTime ?? Time.fixedDeltaTime);

        public void Simulate() => m_rigidbody.velocity += m_direction * Mathf.Abs(GRAVITYFORCE) * m_gravityScale / m_rigidbody.mass * (m_time?.fixedDeltaTime?? Time.fixedDeltaTime);

#if UNITY_EDITOR
        [SerializeField]
        private float m_angle = -90f;
        [SerializeField]
        private bool m_drawDirection;

        public void Validate()
        {
           m_direction = MathfExt.DegreeToVector2(-m_angle);
        }

        public void DrawDirection()
        {
            if (m_drawDirection && m_rigidbody != null)
            {
                var position = (Vector3)m_rigidbody.position;
                Gizmos.DrawLine(position, position + ((Vector3)m_direction * 2f));
            }
        }

        public void Initialize(float scale)
        {
            m_gravityScale = scale;
        }
#endif

    }
}
