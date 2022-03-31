using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Physics
{
    [RequireComponent(typeof(Collider2D))]
    public class CollisionDetector : MonoBehaviour
    {
        [SerializeField]
        private LayerMask m_collisionMask;
        [SerializeField]
        [HideInInspector]
        private Collider2D m_collider;
        [SerializeField]
        private bool m_recordsContactPoints;

        private List<Collider2D> m_otherColliders;
        private List<Collision2D> m_collisions;
        [ShowInInspector, ReadOnly]
        private List<ContactPoint2D> m_contacts;

        public int colliderCount => m_otherColliders?.Count ?? -1;
        public int collisionCount => m_collisions?.Count ?? -1;
        public int contactPointCount => m_contacts?.Count ?? -1;

        public bool recordContactPoints => m_recordsContactPoints;

        public LayerMask collisionMask => m_collisionMask;

        public Collision2D GetCollision(int index)
        {
            if (collisionCount == 0)
            {
                return null;
            }
            else
            {
                return m_collisions[index];
            }
        }
        public void ClearCollisions() => m_collisions.Clear();

        public ContactPoint2D GetContactPoint(int index) => m_contacts[index];
        public void ClearContactPoints() => m_contacts.Clear();

        public void Enable()
        {
            m_collider.enabled = true;
        }

        public void Disable()
        {
            m_collider.enabled = false;
            m_collisions.Clear();
        }


        private void Add(Collision2D collision)
        {
            if (m_collisionMask == (m_collisionMask | 1 << collision.gameObject.layer))
            {
                m_collisions.Add(collision);
                if (m_otherColliders.Contains(collision.collider) == false)
                {
                    m_otherColliders.Add(collision.collider);
                }
            }
        }
        private void Awake()
        {
            m_otherColliders = new List<Collider2D>();
            m_collisions = new List<Collision2D>();
            if (m_recordsContactPoints)
            {
                m_contacts = new List<ContactPoint2D>();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_recordsContactPoints)
            {
                m_contacts.AddRange(collision.contacts);
            }
            Add(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (m_recordsContactPoints)
            {
                for (int i = m_contacts.Count - 1; i >= 0; i--)
                {
                    if(m_contacts[i].collider == collision.collider)
                    {
                        m_contacts.RemoveAt(i);
                    }
                }
                m_contacts.AddRange(collision.contacts);
            }
            for (int i = 0; i < m_collisions.Count; i++)
            {
                if (m_collisions[i].gameObject == collision.gameObject)
                {
                    m_collisions[i] = collision;
                    return;
                }
            }


            //This is incase the collision is not Loaded
            //Add(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            for (int i = 0; i < m_collisions.Count; i++)
            {
                if (m_collisions[i].gameObject == collision.gameObject)
                {
                    m_collisions.RemoveAt(i);
                    return;
                }
            }
            if (m_otherColliders.Contains(collision.collider))
            {
                m_otherColliders.Remove(collision.collider);
            }
            for (int i = m_contacts.Count - 1; i >= 0; i--)
            {
                if (m_contacts[i].collider == collision.collider)
                {
                    m_contacts.RemoveAt(i);
                }
            }
        }

        private void OnValidate()
        {
            m_collider = GetComponent<Collider2D>();
        }

#if UNITY_EDITOR
        public void SetCollisionLayerMask(params string[] layerNames)
        {
            m_collisionMask = LayerMask.GetMask(layerNames);
        }
#endif
    }

}