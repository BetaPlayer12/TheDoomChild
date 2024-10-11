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
        private List<MovableObject> m_movableObjectsToMonitor;
        private Dictionary<MovableObject, Collider2D> m_movableObjectToColliderPair;
        private Coroutine m_monitorMovableObjectsRoutine;


        private void AttackRigidbodyToSelf(Rigidbody2D rigidbody, Collider2D collider)
        {
            var cache = Cache<ParentInfo>.Claim();
            cache.Value.Initialize(rigidbody.transform.parent, rigidbody.gameObject.scene);
            m_originalParentPair.Add(collider, cache);
            rigidbody.transform.parent = m_toParent;
            Debug.Log("Stick");
        }

        private System.Collections.IEnumerator AttackRigidbodyToSelfRoutine(MovableObject movableObject, Rigidbody2D rigidbody, Collider2D collider)
        {
            m_checkedMovableObject = rigidbody;

            while (movableObject.isGrabbed)
                yield return null;

            yield return new WaitForEndOfFrame();
            AttackRigidbodyToSelf(rigidbody, collider);

            m_checkedMovableObject = null;
            m_checkMovableObjectRoutine = null;
        }

        private System.Collections.IEnumerator MonitorMovableObjects()
        {
            do
            {
                Debug.Log("Currently Monitoring Objects");
                if (m_checkMovableObjectRoutine == null)
                {
                    for (int i = 0; i < m_movableObjectsToMonitor.Count; i++)
                    {
                        var movableObject = m_movableObjectsToMonitor[i];
                        if (movableObject.isGrabbed)
                        {
                            var colliderPair = m_movableObjectToColliderPair[movableObject];
                            Debug.Log("Monitor Done");
                            //Remove from original parent pair
                            //TEMPORARILY removed due to new bugs it caused
                            //m_originalParentPair.Remove(colliderPair);
                            m_checkMovableObjectRoutine = StartCoroutine(AttackRigidbodyToSelfRoutine(movableObject, movableObject.GetComponent<Rigidbody2D>(), colliderPair));
                            //We Assume Only 1 Movable Object can be grabbed at all times;
                            break;
                        }
                    }
                }
                yield return null;
            } while (m_movableObjectsToMonitor.Count > 0);

            m_monitorMovableObjectsRoutine = null;
        }

        private void Awake()
        {
            m_originalParentPair = new Dictionary<Collider2D, Cache<ParentInfo>>();
            m_movableObjectsToMonitor = new List<MovableObject>();
            m_movableObjectToColliderPair = new Dictionary<MovableObject, Collider2D>();
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
                            AttackRigidbodyToSelf(collision.rigidbody, collision.collider);
                        }
                        else
                        {
                            if (movableObject.isGrabbed)
                            {
                                if (m_checkMovableObjectRoutine != null)
                                {
                                    StopCoroutine(m_checkMovableObjectRoutine);
                                }
                                m_checkMovableObjectRoutine = StartCoroutine(AttackRigidbodyToSelfRoutine(movableObject, collision.rigidbody, collision.collider));
                            }
                            else
                            {
                                AttackRigidbodyToSelf(collision.rigidbody, collision.collider);
                            }

                            if (m_movableObjectsToMonitor.Contains(movableObject) == false)
                            {
                                m_movableObjectsToMonitor.Add(movableObject);
                                m_movableObjectToColliderPair.Add(movableObject, collision.collider);
                                if (m_monitorMovableObjectsRoutine == null)
                                {
                                    m_monitorMovableObjectsRoutine = StartCoroutine(MonitorMovableObjects());
                                }
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
                        if (m_checkMovableObjectRoutine != null)
                        {
                            StopCoroutine(m_checkMovableObjectRoutine);
                            m_checkMovableObjectRoutine = null;

                            var movableObject = m_checkedMovableObject.GetComponent<MovableObject>();
                            m_movableObjectsToMonitor.Remove(movableObject);
                            m_movableObjectToColliderPair.Remove(movableObject);

                            m_checkedMovableObject = null;
                        }
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
                        Debug.Log("Remove");
                    }
                }
            }
        }
    }
}
