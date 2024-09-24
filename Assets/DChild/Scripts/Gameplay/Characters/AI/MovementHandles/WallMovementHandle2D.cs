using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    [AddComponentMenu("DChild/Gameplay/AI/Movement/Wall Movement Handler 2D")]
    public class WallMovementHandle2D : MovementHandle2D
    {
        [SerializeField]
        protected CharacterPhysics2D m_source;

        public override void MoveTowards(Vector2 direction, float speed)
        {
            if (direction.y != 0)
            {
                var groundDirection = new Vector2(0, Mathf.Sign(direction.y));
                m_source.SetVelocity(groundDirection * speed);
            }
        }

        public override void Stop()
        {
            m_source.SetVelocity(0);
        }
    }
}

