using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class HeavyFalls : MonoBehaviour
    {
        [SerializeField, MinValue(0)]
        private float m_downwardForce;
        [SerializeField, MinValue(0)]
        private float m_horizontalLimit;

        private Dictionary<Rigidbody2D, int> m_rigidbodyReferenceCount;
        private List<Rigidbody2D> m_rigidbodyList;
        private static ContactFilter2D m_objectToGroundFilter;
        private static List<ContactPoint2D> m_contactPoint2Ds;
        private static bool m_staticInitialized;

        private void Awake()
        {
            m_rigidbodyReferenceCount = new Dictionary<Rigidbody2D, int>();
            m_rigidbodyList = new List<Rigidbody2D>();
            if (m_staticInitialized == false)
            {
                m_objectToGroundFilter = new ContactFilter2D();
                m_objectToGroundFilter.useTriggers = false;
                m_objectToGroundFilter.useLayerMask = true;
                m_objectToGroundFilter.SetLayerMask(LayerMask.GetMask("Environment"));
                m_contactPoint2Ds = new List<ContactPoint2D>();
                m_staticInitialized = true;
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < m_rigidbodyList.Count; i++)
            {
                m_contactPoint2Ds.Clear();
                var rigidbody = m_rigidbodyList[i];
                var velocity = rigidbody.velocity;
                var XVelocitySign = Mathf.Sign(velocity.x);
                velocity.x = Mathf.Min(Mathf.Abs(velocity.x), m_horizontalLimit) * XVelocitySign;
                rigidbody.velocity = velocity;

                var contacts = rigidbody.GetContacts(m_objectToGroundFilter, m_contactPoint2Ds);
                bool isInMidAir = true;
                if (contacts > 0)
                {
                    for (int j = 0; j < contacts; j++)
                    {
                        if (Vector2.Angle(Vector2.up, m_contactPoint2Ds[j].normal) <= 45f)
                        {
                            isInMidAir = false;
                            break;
                        }
                    }
                }
                if (isInMidAir)
                {
                    rigidbody.AddForce(Vector3.down * m_downwardForce);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Sensor") == false)
            {
                if (collision.TryGetComponentInParent(out Rigidbody2D rigidbody))
                {
                    if (m_rigidbodyReferenceCount.ContainsKey(rigidbody))
                    {
                        m_rigidbodyReferenceCount[rigidbody] += 1;
                    }
                    else
                    {
                        m_rigidbodyReferenceCount.Add(rigidbody, 1);
                        m_rigidbodyList.Add(rigidbody);
                    }
                }

                if (GameplaySystem.playerManager.IsPartOfPlayer(collision.gameObject,out IPlayer player))
                {
                    player.behaviourModule.SetModuleActive(PrimarySkill.DevilWings, false);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Sensor") == false)
            {
                if (collision.TryGetComponentInParent(out Rigidbody2D rigidbody))
                {
                    if (m_rigidbodyReferenceCount.ContainsKey(rigidbody))
                    {
                        m_rigidbodyReferenceCount[rigidbody] -= 1;
                        if (m_rigidbodyReferenceCount[rigidbody] <= 0)
                        {
                            m_rigidbodyReferenceCount.Remove(rigidbody);
                            m_rigidbodyList.Remove(rigidbody);
                        }
                    }
                }

                if (GameplaySystem.playerManager.IsPartOfPlayer(collision.gameObject, out IPlayer player))
                {
                    player.behaviourModule.SetModuleActive(PrimarySkill.DevilWings, true);
                }
            }
        }
    }
}