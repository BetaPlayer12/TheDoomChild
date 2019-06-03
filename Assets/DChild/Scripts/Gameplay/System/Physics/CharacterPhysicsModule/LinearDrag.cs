using UnityEngine;

namespace DChild.Gameplay.Physics
{
    [System.Serializable]
    public class LinearDrag : ILinearDrag
    {
        [SerializeField]
        [Range(0f, 1f)]
        private float m_x;
        [SerializeField]
        [Range(0f, 1f)]
        private float m_y;

        public float x
        {
            get
            {
                return m_x;
            }

            set
            {
                m_x = Restrict(value);
            }
        }

        public float y
        {
            get
            {
                return m_y;
            }

            set
            {
                m_y = Restrict(value);
            }
        }

        public float SetX
        {
            set
            {
                m_x = value;
            }
        }

        public float SetY
        {
            set
            {
                m_y = value;
            }
        }

        public void AddDrag(float x = float.NaN, float y = float.NaN)
        {
            if (!float.IsNaN(x))
            {
                m_x = Mathf.Min(1, m_x + x);
            }
            if (!float.IsNaN(y))
            {
                m_y = Mathf.Min(1, m_y + y);
            }
        }

        public void ReduceDrag(float x = float.NaN, float y = float.NaN)
        {
            if (!float.IsNaN(x))
            {
                m_x = Mathf.Max(0, m_x - x);
            }
            if (!float.IsNaN(y))
            {
                m_y = Mathf.Max(0, m_y - y);
            }
        }

        public Vector2 ApplyDrag(Vector2 velocity)
        {
            velocity.x -= velocity.x * m_x;
            velocity.y -= velocity.y * m_y;
            return velocity;
        }

        private float Restrict(float value) => Mathf.Max(0, Mathf.Min(1, value));
    }
}
