using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class StickWhileOnPlatform : MonoBehaviour
    {
        [SerializeField]
        private Transform m_toParent;

        private Dictionary<Collider2D, Transform> m_originalParentPair;

        private void Awake()
        {
            m_originalParentPair = new Dictionary<Collider2D, Transform>();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.enabled)
            {
                if (collision.rigidbody != null)
                {
                    if (m_originalParentPair.ContainsKey(collision.collider) == false)
                    {
                        m_originalParentPair.Add(collision.collider, collision.rigidbody.transform.parent);
                        collision.rigidbody.transform.parent = m_toParent;
                    }
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (m_originalParentPair.ContainsKey(collision.collider))
            {
                if (collision.rigidbody != null)
                {
                    collision.rigidbody.transform.parent = m_originalParentPair[collision.collider];
                    m_originalParentPair.Remove(collision.collider);
                }
            }
        }


    }
}
