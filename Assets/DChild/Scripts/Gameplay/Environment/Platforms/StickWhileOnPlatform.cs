using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Environment
{

    public class StickWhileOnPlatform : MonoBehaviour
    {
        public class ParentInfo
        {
            public Transform m_parent;
            public Scene m_scene;

            public void Initialize(Transform parent, Scene scene)
            {
                m_parent = parent;
                m_scene = scene;
            }

            public Transform parent => m_parent;
            public Scene scene => m_scene;
        }

        [SerializeField]
        private Transform m_toParent;

        private Dictionary<Collider2D, Cache<ParentInfo>> m_originalParentPair;

        private Rigidbody2D m_checkedMovableObject;
        private Coroutine m_checkMovableObjectRoutine;

        private void AttackRigidbodyToSelf(Collision2D collision)
        {
            var cache = Cache<ParentInfo>.Claim();
            cache.Value.Initialize(collision.rigidbody.transform.parent, collision.rigidbody.gameObject.scene);
            m_originalParentPair.Add(collision.collider, cache);
            collision.rigidbody.transform.parent = m_toParent;
            Debug.LogError("Stick");
        }

        private System.Collections.IEnumerator AttackRigidbodyToSelfRoutine(MovableObject movableObject, Collision2D collision)
        {
            m_checkedMovableObject = collision.rigidbody;

            while (movableObject.isGrabbed)
                yield return null;

            AttackRigidbodyToSelf(collision);

            m_checkedMovableObject = null;
        }

        private void Awake()
        {
            m_originalParentPair = new Dictionary<Collider2D, Cache<ParentInfo>>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.enabled)
            {
                if (collision.rigidbody != null)
                {
                    if (m_originalParentPair.ContainsKey(collision.collider) == false)
                    {
                        var movableObject = collision.rigidbody.GetComponent<MovableObject>();
                        if (movableObject == null)
                        {
                            AttackRigidbodyToSelf(collision);
                        }
                        else
                        {
                            if (movableObject.isGrabbed)
                            {
                                StopAllCoroutines();
                                m_checkMovableObjectRoutine = StartCoroutine(AttackRigidbodyToSelfRoutine(movableObject, collision));
                            }
                            else
                            {
                                AttackRigidbodyToSelf(collision);
                            }
                        }
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
                    if (m_checkedMovableObject == collision.rigidbody)
                    {
                        StopAllCoroutines();
                        m_checkMovableObjectRoutine = null;
                    }
                    else
                    {
                        var cache = m_originalParentPair[collision.collider];
                        collision.rigidbody.transform.parent = cache.Value.parent;
                        if (cache.Value.parent == null)
                        {
                            SceneManager.MoveGameObjectToScene(collision.rigidbody.gameObject, cache.Value.scene);
                        }
                        cache.Release();
                        m_originalParentPair.Remove(collision.collider);
                        Debug.LogError("Remove");
                    }
                }
            }
        }
    }
}
