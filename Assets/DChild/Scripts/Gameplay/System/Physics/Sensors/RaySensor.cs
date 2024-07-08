using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public interface IRaySensorCastInfo
    {
        bool isDetecting { get; }
        RaycastHit2D[] GetHits();
        RaycastHit2D[] GetUniqueHits();
        RaycastHit2D[] GetValidHits();
        Collider2D GetProminentHitCollider();
    }

    public class RaySensorCastEventArgs : IEventActionArgs
    {
        public IRaySensorCastInfo castInfo { get; private set; }

        public void Initialize(IRaySensorCastInfo castInfo)
        {
            this.castInfo = castInfo;
        }
    }

    public class RaySensor : MonoBehaviour, IRaySensorCastInfo
    {
        public event EventAction<RaySensorCastEventArgs> SensorCast;

        [SerializeField, HideLabel]
        private MultiRaycast m_multiRaycast;
        public MultiRaycast multiRaycast => m_multiRaycast;
        [SerializeField]
        private bool m_enable = true;
#if UNITY_EDITOR
        [SerializeField]
        private bool m_debugMode = true;
#endif

        private Vector2[] m_rayOffsets;
        private float m_prevAngle;

        private RaycastHit2D[] m_uniqueHits;
        private bool m_updatedUniqueHits;
        private RaycastHit2D[] m_validHits;
        private bool m_updatedValidHits;
        private Collider2D m_prominentHitCollider;
        private bool m_updatedProminentHitCollider;


        private static List<Collider2D> m_colliderList;
        private static List<int> m_colliderCountList;
        private static List<RaycastHit2D> m_hitBufferList;
        private static bool m_isInitialized;
        private int m_detectionCount;

        [ShowInInspector, ReadOnly]
        public bool isDetecting => m_multiRaycast.isDetecting;
        [ShowInInspector, ReadOnly]
        public bool allRaysDetecting => m_multiRaycast.areAllRaysDetecting;

        public int DetectionCount => m_detectionCount;

        public RaycastHit2D m_castHit;

        public RaycastHit2D[] GetHits() => m_multiRaycast.hits;
        public List<RaycastHit2D> GetHitsList() => m_multiRaycast.hitsList;
        public RaycastHit2D[] GetUniqueHits()
        {
            if (m_updatedUniqueHits)
            {
                return m_uniqueHits;
            }
            else
            {
                m_colliderList.Clear();
                m_hitBufferList.Clear();
                for (int i = 0; i < m_multiRaycast.count; i++)
                {
                    if (m_colliderList.Contains(m_multiRaycast.hits[i].collider) == false)
                    {
                        m_hitBufferList.Add(m_multiRaycast.hits[i]);
                    }
                }

                m_uniqueHits = m_hitBufferList.ToArray();
                return m_uniqueHits;
            }

        }

        public RaycastHit2D[] GetValidHits()
        {
            if (m_updatedValidHits)
            {
                return m_validHits;
            }
            else
            {
                m_colliderList.Clear();
                m_hitBufferList.Clear();
                for (int i = 0; i < m_multiRaycast.count; i++)
                {
                    if (m_multiRaycast.hits[i].collider != null)
                    {
                        m_hitBufferList.Add(m_multiRaycast.hits[i]);
                    }
                }

                m_validHits = m_hitBufferList.ToArray();
                return m_validHits;
            }
        }

        public Collider2D GetProminentHitCollider()
        {
            if (m_updatedProminentHitCollider)
            {
                return m_prominentHitCollider;
            }
            else
            {
                m_colliderList.Clear();
                m_colliderCountList.Clear();
                Collider2D toCheck = null;
                int highestCollliderCount = 0;
                for (int i = 0; i < m_multiRaycast.count; i++)
                {
                    toCheck = m_multiRaycast.hits[i].collider;
                    if (m_colliderList.Contains(m_multiRaycast.hits[i].collider))
                    {
                        for (int j = 0; j < m_colliderList.Count; j++)
                        {
                            if (m_colliderList[j] == toCheck)
                            {
                                m_colliderCountList[j] += 1;
                                if (m_colliderCountList[j] > highestCollliderCount)
                                {
                                    highestCollliderCount = m_colliderCountList[j];
                                    m_prominentHitCollider = toCheck;
                                }
                            }
                        }
                    }
                    else
                    {
                        m_colliderList.Add(toCheck);
                        m_colliderCountList.Add(1);

                        if (m_prominentHitCollider == null)
                        {
                            m_prominentHitCollider = toCheck;
                            highestCollliderCount = 1;
                        }
                    }
                }
                return m_prominentHitCollider;
            }
        }

        public void SetRotation(float angle)
        {
            if (m_prevAngle != angle)
            {
                transform.localRotation = Quaternion.Euler(0, 0, angle);
                m_prevAngle = angle;
                Cast();
            }
        }

        public void Cast()
        {
            if (m_enable)
            {
#if UNITY_EDITOR
                m_multiRaycast.Cast(transform.position, transform.right, m_debugMode);
#else
                m_multiRaycast.Cast(transform.position, transform.right);
#endif
                m_uniqueHits = null;
                m_updatedUniqueHits = false;
                m_validHits = null;
                m_updatedValidHits = false;
                m_prominentHitCollider = null;
                m_updatedProminentHitCollider = false;
                using (Cache<RaySensorCastEventArgs> cacheEventArgs = Cache<RaySensorCastEventArgs>.Claim())
                {
                    cacheEventArgs.Value.Initialize(this);
                    SensorCast?.Invoke(this, cacheEventArgs.Value);
                    cacheEventArgs.Release();
                }
            }
        }

        private void Awake()
        {
            if (m_isInitialized == false)
            {
                m_colliderList = new List<Collider2D>();
                m_colliderCountList = new List<int>();
                m_hitBufferList = new List<RaycastHit2D>();
                m_isInitialized = true;
            }

            m_multiRaycast.UseConfiguration();
            m_rayOffsets = new Vector2[m_multiRaycast.count];
            m_prevAngle = transform.localRotation.eulerAngles.z;
        }

#if UNITY_EDITOR
        public void Initialize(int count, LayerMask mask, bool ignoreTrigger, float castWidth, float castDistance)
        {
        }

        public void Initialize(float rotation)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        private void OnDrawGizmosSelected()
        {
            m_multiRaycast.DrawGizmo(transform.position, transform.up, transform.right);
        }
#endif
    }

}