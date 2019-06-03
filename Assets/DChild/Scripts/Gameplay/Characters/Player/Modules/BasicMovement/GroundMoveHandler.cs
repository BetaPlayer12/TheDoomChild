using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    [System.Serializable]
    public class GroundMoveHandler : MoveHandler
    {
        public GroundMoveHandler(float m_currentMaxSpeed, float m_currentAcceleration, float m_currentDecceleration) : base(m_currentMaxSpeed, m_currentAcceleration, m_currentDecceleration)
        {
        }

        public void IncreaseMoveVelocity(float sprintSpeed, float sprintAcceleration, float sprintDecceleration)
        {
            m_currentMaxSpeed = sprintSpeed;
            m_currentAcceleration = sprintAcceleration;
            m_currentDecceleration = sprintDecceleration;
        }

        public void ResetMoveVelocity(float jogSpeed, float jogAcceleration, float jogDecceleration)
        {
            m_currentMaxSpeed = jogSpeed;
            m_currentAcceleration = jogAcceleration;
            m_currentDecceleration = jogDecceleration;
        }

        protected override Vector2 ConvertToMoveForce(float acceleration) => m_characterPhysics.moveAlongGround * (acceleration * m_direction.x);

        protected override bool IsInMaxSpeed()
        {
            if (m_direction.x > 0)
            {
                return m_characterPhysics.velocity.x < m_currentMaxSpeed * m_characterPhysics.moveAlongGround.x;
            }
            else
            {
                return m_characterPhysics.velocity.x > -m_currentMaxSpeed * m_characterPhysics.moveAlongGround.x;
            }
        }
    } 
}

