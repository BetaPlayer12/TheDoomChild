using UnityEngine;

namespace DChild.Gameplay
{
    public class VelocityRestrictor : MonoBehaviour
    {
        [SerializeField]
        private Vector2 m_minVelocity = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
        [SerializeField]
        private Vector2 m_maxVelocity = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

        private Rigidbody2D m_rigidbody;

        public void SetMinVelocity(float x = float.NaN, float y = float.NaN) => SetVelocity(ref m_minVelocity, x, y);

        public void SetMaxVelocity(float x = float.NaN, float y = float.NaN) => SetVelocity(ref m_maxVelocity, x, y);

        public void SetMinVelocity(Vector2 velocity) => m_minVelocity = velocity;

        public void SetMaxVelocity(Vector2 velocity) => m_maxVelocity = velocity;

        private void SetVelocity(ref Vector2 velocity, float x = float.NaN, float y = float.NaN)
        {
            AssignValidFloat(ref velocity.x, x);
            AssignValidFloat(ref velocity.y, y);
        }

        private void AssignValidFloat(ref float reference, float value)
        {
            if (!float.IsNaN(value))
            {
                reference = value;
            }
        }

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var velocity = m_rigidbody.velocity;
            velocity.x = Mathf.Clamp(velocity.x, m_minVelocity.x, m_maxVelocity.x);
            velocity.y = Mathf.Clamp(velocity.y, m_minVelocity.y, m_maxVelocity.y);
            m_rigidbody.velocity = velocity;
        }

#if UNITY_EDITOR
        public void Initialize(float minVelocityX, float minVelocityY, float maxVelocityX, float maxVelocityY)
        {
            m_minVelocity = new Vector2 (minVelocityX, minVelocityY);
            m_maxVelocity = new Vector2 (maxVelocityX, maxVelocityY);
        }
#endif
    }
}