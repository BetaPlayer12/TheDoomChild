
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class IsolatedObjectPhysics2D : ObjectPhysics2D
    {
        public sealed override Vector2 velocity
        {
            get
            {
                return m_rigidbody2D.velocity;
            }

            protected set
            {
                m_rigidbody2D.velocity = value;
            }
        }

        protected override Vector2 up => Vector2.up;

        protected override void Awake()
        {
            base.Awake();
            m_gravity.SetAngle(Vector2.down);
        }
    }
}
