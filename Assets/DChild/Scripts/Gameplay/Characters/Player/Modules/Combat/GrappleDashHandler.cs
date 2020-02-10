using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    [System.Serializable]
    public struct GrappleDashHandler
    {
        [SerializeField]
        public float m_dashSpeed;

        private Vector2 m_destination;
        private Vector2 m_direction;

        public void Set(Vector2 position, Vector2 destination)
        {
            m_destination = destination;
            m_direction = (m_destination - position).normalized;
        }

        public void DashToDestination(CharacterPhysics2D physics, Vector2 position, ref bool dashState)
        {
            if (Vector2.Distance(position, m_destination) <= 0.5f)
            {
                dashState = false;
                var velocity = physics.velocity;
                velocity.y = 0;
                physics.SetVelocity(velocity);
            }
            else
            {
                physics.SetVelocity(m_direction * m_dashSpeed);
            }
        }
    }
}   