using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Modules
{
    public class AcceleratedAirMovement : MonoBehaviour, IMoveHandle
    {
#if UNITY_EDITOR
        [SerializeField]
#endif
        private IsolatedPhysics2D m_physics;

        [SerializeField, MinValue(0)]
        private float m_topSpeed;
        [SerializeField, MinValue(0)]
        private float m_acceleration;
        [SerializeField, MinValue(0)]
        private float m_deceleration;

        private HorizontalDirection m_moveDirection;
        private bool m_isMoving;

        public void Move(HorizontalDirection direction)
        {
            if (m_moveDirection != direction)
            {
                var velocity = m_physics.velocity;
                m_physics.SetVelocity(-velocity.x);
                m_moveDirection = direction;
            }

            if (HasReachedTopSpeed())
            {
                m_physics.SetVelocity((int)direction * m_topSpeed);
            }
            else
            {
                var directionVector = new Vector2((int)direction, 0);
                m_physics.AddVelocity(m_acceleration * directionVector);
            }
            m_isMoving = true;
        }

        public void Stop()
        {
            if (m_isMoving)
            {
                if (m_moveDirection == HorizontalDirection.Left)
                {
                    if (m_physics.velocity.x < 0)
                    {
                        m_physics.AddForce(m_deceleration * Vector2.right);
                    }
                    else
                    {
                        m_physics.SetVelocity(0);
                        m_isMoving = false;
                    }
                }
                else
                {
                    if (m_physics.velocity.x > 0)
                    {
                        m_physics.AddForce(m_deceleration * Vector2.left);
                    }
                    else
                    {
                        m_physics.SetVelocity(0);
                        m_isMoving = false;
                    }
                }
            }
        }

        private bool HasReachedTopSpeed()
        {
            if (m_moveDirection == HorizontalDirection.Left)
            {
                return m_physics.velocity.x <= -m_topSpeed;
            }
            else
            {
                return m_physics.velocity.x >= m_topSpeed;
            }
        }
    }
}