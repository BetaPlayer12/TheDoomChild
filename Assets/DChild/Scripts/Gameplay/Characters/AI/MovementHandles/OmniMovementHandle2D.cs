using DChild.Gameplay;
using UnityEngine;

namespace DChild.Gameplay
{
    [AddComponentMenu("DChild/Gameplay/AI/Movement/Omni Movement Handler 2D")]
    public class OmniMovementHandle2D : MovementHandle2D
    {
        [SerializeField]
        protected IsolatedPhysics2D m_source;

        public override void MoveTowards(Vector2 direction, float speed) => m_source.SetVelocity(direction.normalized * speed);

        public override void Stop() => m_source.SetVelocity(Vector2.zero);
    }
}