using Holysoft;
using Holysoft.Collections;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DChild.Gameplay.Environment
{
    [AddComponentMenu("DChild/Gameplay/Environment/Debris")]
    public class Debris : MonoBehaviour
    {
        [SerializeField, MinValue(0)]
        private float m_angleOffset;
        [SerializeField, MinValue(0)]
        private float m_forceOffset;

        [SerializeField, MinValue(1)]
        private float m_duration;
        [SerializeField, MinValue(0.1f)]
        private float m_fadeDuration;

        [SerializeField, ValueDropdown("GetRigidbodies", IsUniqueList = true)]
        private Rigidbody2D[] m_toDetach;

        private List<Rigidbody2D> m_rigidbodies;
        private List<float> m_durationTimer;
        private List<SpriteRenderer> m_renderers;

        private static Rigidbody2D m_cacheRigidbody;

        [ShowInInspector, HideInPrefabAssets]
        private float m_force;
        [SerializeField, HideInPrefabAssets]
        private float m_angle;
        private float m_fadeSpeed;

        private int m_trackedObjectCount;

        public Rigidbody2D[] GetDetachables()
        {
            if (m_toDetach.Length > 0)
            {
                return m_toDetach;
            }
            else
            {
                return null;
            }
        }

        public void SetInitialForceReference(Vector2 forceDirection, float force)
        {
            m_angle = Vector2.Angle(Vector2.right, forceDirection) * Mathf.Sign(forceDirection.y);
            m_force = force;
        }


        private void Awake()
        {
            m_rigidbodies = new List<Rigidbody2D>(GetComponentsInChildren<Rigidbody2D>());
            m_durationTimer = new List<float>();
            m_renderers = new List<SpriteRenderer>();
            m_fadeSpeed = 1 / m_fadeDuration;
        }

        private void Start()
        {
            for (int i = 0; i < m_rigidbodies.Count; i++)
            {
                var force = m_force + Random.Range(-m_forceOffset, m_forceOffset);
                var angle = MathfExt.DegreeToVector2(m_angle + Random.Range(-m_angleOffset, m_angleOffset));
                m_cacheRigidbody = m_rigidbodies[i];
                m_cacheRigidbody.AddForce(angle * force, ForceMode2D.Impulse);
                m_cacheRigidbody.AddTorque(angle.x * force, ForceMode2D.Impulse);
            }

            for (int i = 0; i < m_toDetach.Length; i++)
            {
                m_toDetach[i].transform.SetParent(null);
                m_rigidbodies.Remove(m_toDetach[i]);
            }
            for (int i = 0; i < m_rigidbodies.Count; i++)
            {
                m_durationTimer.Add(m_duration);
            }
            m_trackedObjectCount = m_rigidbodies.Count;
        }

        private void LateUpdate()
        {
            var DeltaTime = Time.deltaTime;
            for (int i = m_rigidbodies.Count - 1; i >= 0; i--)
            {
                m_cacheRigidbody = m_rigidbodies[i];
                //if (m_cacheRigidbody.IsSleeping())
                //{
                m_durationTimer[i] -= DeltaTime;
                if (m_durationTimer[i] <= 0)
                {
                    m_renderers.Add(m_cacheRigidbody.GetComponentInChildren<SpriteRenderer>());
                    m_durationTimer.RemoveAt(i);
                    m_rigidbodies.RemoveAt(i);
                }
                //}
            }
            for (int i = m_renderers.Count - 1; i >= 0; i--)
            {
                var color = m_renderers[i].color;
                color.a -= m_fadeSpeed * DeltaTime;
                m_renderers[i].color = color;
                if (color.a <= 0)
                {
                    m_renderers.RemoveAt(i);
                    m_trackedObjectCount--;
                    if (m_trackedObjectCount <= 0)
                    {
                        Addressables.ReleaseInstance(gameObject);
                    }
                }
            }
        }

#if UNITY_EDITOR
        private IEnumerable GetRigidbodies()
        {
            return GetComponentsInChildren<Rigidbody2D>();
        }
#endif
    }
}