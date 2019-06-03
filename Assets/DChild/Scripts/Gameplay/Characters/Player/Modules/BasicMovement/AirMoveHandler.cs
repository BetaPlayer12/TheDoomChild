using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    [System.Serializable]
    public class AirMoveHandler : MoveHandler
    {
        public AirMoveHandler(float m_currentMaxSpeed, float m_currentAcceleration, float m_currentDecceleration) : base(m_currentMaxSpeed, m_currentAcceleration, m_currentDecceleration)
        {
        }

        protected override Vector2 ConvertToMoveForce(float acceleration) => acceleration * m_direction;

        protected override bool IsInMaxSpeed()
        {
            if (m_direction.x > 0)
            {
                return m_characterPhysics.velocity.x < m_currentMaxSpeed;
            }
            else
            {
                return m_characterPhysics.velocity.x > -m_currentMaxSpeed;
            }
        }
    }
} 