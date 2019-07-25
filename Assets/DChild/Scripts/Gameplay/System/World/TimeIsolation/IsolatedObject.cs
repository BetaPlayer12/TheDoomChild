/***************************************
 * 
 * This class is use to isolate Time to each object
 * 
 ***************************************/

using Sirenix.OdinInspector;
using UnityEngine;

using DChild.Gameplay.Systems.WorldComponents;
using Spine.Unity;
using System.Collections.Generic;

namespace DChild.Gameplay.Systems.WorldComponents
{

    [System.Serializable]
    public class IsolatedObject : MonoBehaviour, IIsolatedObject, IIsolatedTimeModifier, IIsolatedTime, IIsolatedPhysicsTime
    {
        [SerializeField]
        private bool m_registerToWorldTime = true;
        [SerializeField, MinValue(0f)]
        private float m_slowFactor = 0;
        [SerializeField, MinValue(0f)]
        protected float m_fastFactor = 0;

        [SerializeField]
        [HideInInspector]
        private FXObjects m_fXObjects;
        [SerializeField]
        [HideInInspector]
        private SpineObjects m_spineObjects;
        [SerializeField]
        [HideInInspector]
        private PhysicsObjects m_physicsObjects;
        [SerializeField]
        [HideInInspector]
        private Material[] m_materials;

        private float m_timeScale;
        private float m_totalTimeScale;
        private float m_deltaTime;
        private float m_fixedDeltaTime;
        private bool m_componentTimeAligned;
        private bool m_physicsTimeAligned;
        private bool m_timeStopped;

        public float slowFactor => m_slowFactor;
        public float fastFactor => m_fastFactor;

        public float timeScale => m_timeScale;
        public float totalTimeScale => m_timeStopped ? 0 : m_totalTimeScale;

        public float deltaTime
        {
            get
            {
                if (m_timeStopped)
                {
                    return 0;
                }
                else
                {
                    if (m_registerToWorldTime)
                    {
                        return m_deltaTime;
                    }
                    else
                    {
                        return m_totalTimeScale == Time.timeScale ? Time.deltaTime : GameplaySystem.world.CalculateDeltaTime(m_totalTimeScale);
                    }
                }
            }
        }

        public float fixedDeltaTime
        {
            get
            {
                if (m_timeStopped)
                {
                    return 0;
                }
                else
                {
                    if (m_registerToWorldTime)
                    {
                        return m_fixedDeltaTime;
                    }
                    else
                    {
                        return m_totalTimeScale == Time.timeScale ? Time.fixedDeltaTime : GameplaySystem.world.CalculateFixedDeltaTime(m_totalTimeScale);
                    }
                }
            }
        }

#if UNITY_EDITOR
        public int componentCount => 0;
#endif

        public void UpdateDeltaTime()
        {
            m_deltaTime = m_totalTimeScale == GameplaySystem.time.timeScale ? GameplaySystem.time.deltaTime : GameplaySystem.world.CalculateDeltaTime(m_totalTimeScale);
            m_fixedDeltaTime = m_totalTimeScale == GameplaySystem.time.timeScale ? GameplaySystem.time.fixedDeltaTime : GameplaySystem.world.CalculateFixedDeltaTime(m_totalTimeScale);
        }

        public void CalculateActualVelocity()
        {
            m_physicsObjects?.CalculateActualVelocity(m_totalTimeScale);
        }

        public void StopTime()
        {
            m_timeStopped = true;
            QueueforUpdates();
        }

        public void ResumeTime()
        {
            m_timeStopped = false;
            QueueforUpdates();
        }

        public void Faster(float percentValue)
        {
            m_fastFactor = percentValue;
            UpdateTimeScale();
            QueueforUpdates();
        }

        public void SetTimeScale(float timeScale)
        {
            CalculateActualVelocity();
            m_timeScale = timeScale;
            UpdateTimeScale();
            QueueforUpdates();
        }
        public void Slower(float percentValue)
        {
            m_slowFactor = percentValue;
            UpdateTimeScale();
            QueueforUpdates();
        }

        public void UpdateComponents()
        {
            if (m_componentTimeAligned == false)
            {
                m_fXObjects?.AlignTime(m_totalTimeScale);
                if (m_materials != null)
                {
                    RendererTimeHandler.AlignTime(m_materials, m_totalTimeScale);
                }
                m_spineObjects?.AlignTime(m_totalTimeScale);
                m_componentTimeAligned = true;
            }
        }

        public void UpdatePhysicsComponent()
        {
            if (m_physicsTimeAligned == false)
            {
                m_physicsObjects?.AlignTime(m_totalTimeScale);
                m_physicsTimeAligned = true;
            }
        }

        public Vector2 GetRelativeForce(Vector2 velocity) => velocity * m_totalTimeScale;
        public float GetRelativeForce(float force) => force * m_totalTimeScale;

        protected void UpdateTimeScale() => m_totalTimeScale = m_timeScale * (1f + (m_fastFactor - m_slowFactor));

        private void QueueforUpdates()
        {
            if (m_componentTimeAligned)
            {
                QueueForComponentUpdate();
            }
            if (m_physicsTimeAligned)
            {
                QueueForPhysicsComponentUpdate();
            }
        }

        private void QueueForComponentUpdate()
        {
            if (m_registerToWorldTime)
            {
                GameplaySystem.world.RequestUpdateComponentsOf(this);
                m_componentTimeAligned = false;
            }
            else
            {
                m_componentTimeAligned = false;
                UpdateComponents();
            }
        }

        private void QueueForPhysicsComponentUpdate()
        {
            if (m_registerToWorldTime)
            {
                GameplaySystem.world.RequestUpdatePhysicsComponentsOf(this);
                m_physicsTimeAligned = false;
            }
            else
            {
                m_physicsTimeAligned = false;
                UpdatePhysicsComponent();
            }
        }

        private void Awake()
        {
            m_timeScale = 1f;
            UpdateTimeScale();
            m_componentTimeAligned = true;
            m_physicsTimeAligned = true;
            m_timeStopped = false;
        }

        private void OnEnable()
        {
            CalculateActualVelocity();
            if (m_registerToWorldTime)
            {
                GameplaySystem.world.Register(this);
            }
        }

        private void OnDisable()
        {
            if (m_registerToWorldTime)
            {
                GameplaySystem.world.Unregister(this);
            }
        }

        private void OnValidate()
        {
            var particleSystems = GetComponentsInChildren<ParticleSystem>();
            if (particleSystems == null)
            {
                m_fXObjects = null;
            }
            else
            {
                m_fXObjects = new FXObjects(particleSystems);
            }

            var spineAnimations = GetComponentsInChildren<SkeletonAnimation>();
            if (spineAnimations == null)
            {
                m_spineObjects = null;
            }
            else
            {
                m_spineObjects = new SpineObjects(spineAnimations);
            }

            var rigidbodies = GetComponentsInChildren<Rigidbody2D>();
            if (rigidbodies == null)
            {
                m_physicsObjects = null;
            }
            else
            {
                m_physicsObjects = new PhysicsObjects(rigidbodies);
            }

            var renderers = GetComponentsInChildren<Renderer>();
            if (renderers == null)
            {
                m_materials = null;
            }
            else
            {
                List<Material> materialList = new List<Material>();
                List<int> countPerMaterialList = new List<int>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    var material = renderers[i].sharedMaterial;
                    if (material != null && material.HasProperty("_TimeScale"))
                    {
                        materialList.Add(material);
                    }
                }
                materialList.RemoveAll(x => x == null);
                if (materialList.Count == 0)
                {
                    m_materials = null;
                }
                else
                {
                    m_materials = materialList.ToArray();

                }
            }
        }
    }
}
