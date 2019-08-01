using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

    [System.Serializable]
    public abstract class MoveHandler
    {
        [SerializeField]
        protected float m_currentMaxSpeed;
        [SerializeField]
        protected float m_currentAcceleration;
        [SerializeField]
        protected float m_currentDecceleration;

        protected CharacterPhysics2D m_characterPhysics;
        protected Vector2 m_direction;
        protected const float TOLERABLESTOPVELOCITY = 0.1f;

        protected MoveHandler(float m_maxSpeed, float m_acceleration, float m_decceleration)
        {
            this.m_currentMaxSpeed = m_maxSpeed;
            this.m_currentAcceleration = m_acceleration;
            this.m_currentDecceleration = m_decceleration;
        }

        public void SetPhysics(CharacterPhysics2D dynamicBody2D) => m_characterPhysics = dynamicBody2D;

        public void SetDirection(Vector2 direction) => m_direction = direction;

        public void Accelerate()
        {
            if (IsInMaxSpeed())
            {
                var force = ConvertToMoveForce(m_currentAcceleration);
                m_characterPhysics.AddForce(force);
            }

        }

        public void Deccelerate()
        {

            if (m_direction.x > 0)
            {

                if (m_characterPhysics.velocity.x > TOLERABLESTOPVELOCITY)
                {
                    m_characterPhysics.AddForce(Vector2.left * m_currentDecceleration);
                    if (m_characterPhysics.velocity.x <= 0)
                    {
                        m_characterPhysics.SetVelocity(0);
                    }
                }

            }
            else
            {

                if (m_characterPhysics.velocity.x < -TOLERABLESTOPVELOCITY)
                {

                    m_characterPhysics.AddForce(Vector2.right * m_currentDecceleration);
                    if (m_characterPhysics.velocity.x >= 0)
                    {
                        m_characterPhysics.SetVelocity(0);
                    }
                }
            }


        }

        protected abstract bool IsInMaxSpeed();
        protected abstract Vector2 ConvertToMoveForce(float acceleration);
    }

}
