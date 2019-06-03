using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public abstract class ColliderIntersectDetector : MonoBehaviour
    {
        protected List<Collider2D> m_intersectingColliders;

        public int intersectingColliderCount => m_intersectingColliders.Count;
        public Collider2D GetIntersectingCollider(int index) => m_intersectingColliders[index];

        public abstract bool IsIntersecting();
        public abstract bool IsIntersecting(Collider2D collider2D);
    }
}
