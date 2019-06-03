using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PhysicsMovementHandler2D
    {
        private IsolatedPhysics2D m_physics;
        private Transform transform;

        private static bool isInitialized;
        private static Vector2 moveGroundDirection;
        private static Vector2 forwardGroundDirection;
        private static Vector2 backwardGroundDirection;

        public enum GroundOrientation
        {
            horizontal,
            vertical
        };

        private GroundOrientation m_orientation;

        public PhysicsMovementHandler2D(IsolatedPhysics2D m_physics, Transform transform)
        {
            this.m_physics = m_physics;
            this.transform = transform;

            if (isInitialized == false)
            {
                isInitialized = true;
                moveGroundDirection = new Vector2(1, 0);
                forwardGroundDirection = new Vector2(1, 0);
                backwardGroundDirection = new Vector2(-1, 0);
            }
        }

        public void MoveTo(Vector2 target, float speed)
        {
            var direction = (target - (Vector2)transform.position).normalized;
            m_physics.SetVelocity(direction * speed);
        }

        public void MoveOnGround(Vector2 target, float speed)
        {
            //var direction = (target - (Vector2)transform.position).normalized;
            //m_physics.SetVelocity((Mathf.Sign(direction.x) * moveGroundDirection) * speed);
            if (target.x > transform.position.x)
            {
                m_physics.SetVelocity(forwardGroundDirection * speed);
            }
            else
            {
                m_physics.SetVelocity(backwardGroundDirection * speed);
            }
        }


        public void MoveTowards(Vector2 direction, float speed) => m_physics.SetVelocity(direction * speed);

        public void Stop() => m_physics.SetVelocity(Vector2.zero);
    }

}