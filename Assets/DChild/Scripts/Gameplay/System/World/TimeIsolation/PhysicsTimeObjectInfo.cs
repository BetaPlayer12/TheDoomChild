using UnityEngine;

namespace DChild.Gameplay.Systems.WorldComponents
{
    [System.Serializable]
    public struct PhysicsTimeObjectInfo
    {
        [SerializeField]
        private float m_mass;
        [SerializeField]
        private Vector2 m_actualVelocity;

        public PhysicsTimeObjectInfo(Rigidbody2D rigibody2D) : this()
        {
            m_mass = rigibody2D.mass;
            m_actualVelocity = rigibody2D.velocity;       
        }

        public void AlignTime(Rigidbody2D rigidbody2D, float timeScale)
        {
            if (rigidbody2D != null)
            {
                rigidbody2D.mass = m_mass / timeScale;
                rigidbody2D.velocity = m_actualVelocity * timeScale;
            }          
        }

        public void CalculateActualVelocity(Rigidbody2D rigidbody2D, float timeScale)
        {
            m_actualVelocity = rigidbody2D.velocity / timeScale;
            if (float.IsNaN(m_actualVelocity.x))
            {
                m_actualVelocity.x = 0;
            }
            if (float.IsNaN(m_actualVelocity.y))
            {
                m_actualVelocity.y = 0;
            }
        }

        public void Revert(Rigidbody2D rigidbody2D)
        {
            rigidbody2D.mass = m_mass;
            rigidbody2D.velocity = m_actualVelocity;
        }
    }

}