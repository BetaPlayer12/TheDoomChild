using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    public class ShiftingObjectPhysics2D : ObjectPhysics2D, IWorldShifter
    {
        [SerializeField]
        [TabGroup("Configuration")]
        private WorldShifter m_worldShifter;

        public override Vector2 velocity
        {
            get
            {
               return m_worldShifter.velocity;
            }

            protected set
            {
                m_rigidbody2D.velocity = m_worldShifter.ConvertToOrientation(value);
            }
        }

        protected override Vector2 up => m_worldShifter.up;

        public void SetOrientation(WorldOrientation worldOrientation)
        {
            m_worldShifter.SetOrientation(worldOrientation);
            m_gravity.SetAngle(-m_worldShifter.up);

            switch (worldOrientation)
            {
                case WorldOrientation.Up:
                    m_rigidbody2D.rotation = 0f;
                    break;
                case WorldOrientation.Down:
                    m_rigidbody2D.rotation = -180f;
                    break;
                case WorldOrientation.Right:
                    m_rigidbody2D.rotation = -90f;
                    break;
                case WorldOrientation.Left:
                    m_rigidbody2D.rotation = -270f;
                    break;
            }
        }

        public override void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
        {
            var shiftedForce = m_worldShifter.ConvertToOrientation(force);
            base.AddForce(shiftedForce, mode);
        }

        public override void AddExplosion(float force, Vector2 explisionPosition, float radius, float upliftModified, bool selfImmune = true)
        {
            var shiftedExplosion = m_worldShifter.ConvertToOrientation(explisionPosition);
            base.AddExplosion(force, shiftedExplosion, radius, upliftModified, selfImmune);
        }

        protected override void Awake()
        {
            base.Awake();
            m_worldShifter.Initialize(m_rigidbody2D, m_gravity);
            m_worldShifter.SetOrientation(m_worldShifter.orientation);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
            m_worldShifter.Initialize(m_rigidbody2D, m_gravity);
            m_worldShifter.SetOrientation(m_worldShifter.orientation);
        }
#endif
    }
}
