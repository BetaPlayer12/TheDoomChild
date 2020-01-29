using DChild.Gameplay;
using UnityEngine;

namespace DChild.Gameplay
{
    [AddComponentMenu("DChild/Gameplay/AI/Movement/Ground Movement Handler 2D")]
    public class GroundMovementHandler2D : MovementHandle2D
    {
        [SerializeField]
        protected CharacterPhysics2D m_source;

        public override void MoveTowards(Vector2 direction, float speed)
        {
            if (direction.x != 0)
            {
                var groundDirection = new Vector2(Mathf.Sign(direction.x), 0);
                m_source.SetVelocity(groundDirection * speed);
            }
        }

        public override void Stop()
        {
            m_source.SetVelocity(0);
        }

#if UNITY_EDITOR
        public void InitializeField(CharacterPhysics2D physics)
        {
            m_source = physics;
        }
#endif
    }

}