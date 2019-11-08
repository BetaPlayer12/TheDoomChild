using UnityEngine;

namespace DChild.Gameplay
{
    [AddComponentMenu("DChild/Gameplay/Physics/Isolated Character Physics 2D")]
    public class IsolatedCharacterPhysics2D : CharacterPhysics2D
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
