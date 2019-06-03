using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Physics
{
    public class CapsuleColliderDetector : ColliderIntersectDetector
    {
        [SerializeField]
        private LayerMask m_mask;
        private CapsuleDirection2D m_direction;

        public override bool IsIntersecting(Collider2D collider)
        {
            if (collider.bounds.extents.x < collider.bounds.extents.y)
            {
                m_direction = CapsuleDirection2D.Vertical;
            }
            else
            {
                m_direction = CapsuleDirection2D.Horizontal;
            }
            var angle = collider.transform.eulerAngles.z;
            var m_collider = Physics2D.OverlapCapsule(collider.bounds.center, collider.bounds.size, m_direction, angle, m_mask);
           
            if(m_collider != null)
            {
                m_intersectingColliders.Add(m_collider);
            }
            else
            {
                for(int x = m_intersectingColliders.Count; x > 0; x--)
                {
                    m_intersectingColliders.RemoveAt(x-1);
                }
            }

            return m_intersectingColliders.Count > 0;
        }

        public override bool IsIntersecting()
        {
            throw new System.NotImplementedException();
        }

        private void Awake()
        {
            m_intersectingColliders = new List<Collider2D>();
        }
    }
}

