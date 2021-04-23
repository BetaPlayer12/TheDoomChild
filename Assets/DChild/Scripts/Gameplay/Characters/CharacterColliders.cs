using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    [AddComponentMenu("DChild/Gameplay/Object/Character Colliders")]
    public class CharacterColliders : MonoBehaviour
    {
        [SerializeField]
        private Collider2D[] m_colliders;
        private ColliderIntersectDetector[] m_detectors;
        private List<Collider2D> m_ignoredCollisions;

        public Collider2D[] colliders => m_colliders;
        public ColliderIntersectDetector[] detectors => m_detectors;


        public void IgnoreCollider(Collider2D collider)
        {
            for (int i = 0; i < m_colliders.Length; i++)
            {
                Physics2D.IgnoreCollision(collider, m_colliders[i], true);
                m_ignoredCollisions.Add(collider);
            }
        }

        public void ClearIgnoredCollider(Collider2D collider)
        {
            for (int i = 0; i < m_colliders.Length; i++)
            {
                for (int j = 0; j < m_ignoredCollisions.Count; j++)
                {
                    Physics2D.IgnoreCollision(m_ignoredCollisions[j], m_colliders[i], false);
                    m_ignoredCollisions.Remove(m_ignoredCollisions[j]);
                }
            }
        }

        public bool AreCollidersIntersecting()
        {
            for (int i = 0; i < m_detectors.Length; i++)
            {
                for (int j = 0; j < m_detectors[i].intersectingColliderCount; j++)
                {
                    if (m_detectors[i].IsIntersecting(m_colliders[j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AreCollidersIntersecting(Collider2D otherCollider)
        {
            for (int i = 0; i < m_detectors.Length; i++)
            {
                for (int j = 0; j < m_detectors[i].intersectingColliderCount; j++)
                {
                    if (m_detectors[i].IsIntersecting(otherCollider))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Enable()
        {
            for (int i = 0; i < m_colliders.Length; i++)
            {
                m_colliders[i].enabled = true;
            }
        }

        public void Disable()
        {
            for (int i = 0; i < m_colliders.Length; i++)
            {
                m_colliders[i].enabled = false;
            }
        }

        private void Awake()
        {
            m_ignoredCollisions = new List<Collider2D>();
            m_detectors = GetComponentsInChildren<ColliderIntersectDetector>();
        }

        [Button]
        public void UseChildrenColliders()
        {
            m_colliders = GetComponentsInChildren<Collider2D>();
        }
    }
}