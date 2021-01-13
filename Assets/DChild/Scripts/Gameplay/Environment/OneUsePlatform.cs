using DChild.Gameplay.Characters;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class OneUsePlatform : MonoBehaviour,ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            public SaveData(bool isDisappeared) : this()
            {
                this.m_hasDisappeared = isDisappeared;
            }

            [SerializeField]
            private bool m_hasDisappeared;

            public bool isDisappeared => m_hasDisappeared;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_hasDisappeared);
        }
        [SerializeField]
        private DisappearingPlatformData m_disappearingPlatformData;
        [SerializeField, TabGroup("OnDisappear")]
        private UnityEvent m_onDisappear;
        private SkeletonAnimation m_animation;
        private Collider2D m_collider;
        private bool m_willDisappear = false;
        private bool m_hasDisappeared = false;
        private float m_disappearDelayTimer;
        private bool m_hasReactivePlatform;

        public ISaveData Save() => new SaveData(m_hasDisappeared);

        public void Load(ISaveData data)
        {
            var saveData = (SaveData)data;
            m_hasDisappeared = saveData.isDisappeared;
            if (m_hasDisappeared)
            {
                m_onDisappear?.Invoke();
            }
           
        }

        private void DisappearPlatform()
        {
            m_willDisappear = true;
            m_disappearDelayTimer = m_disappearingPlatformData.disappearDelay;
            if (m_animation != null)
            {
                m_animation.state.SetAnimation(0, m_disappearingPlatformData.steppedOnAnimation, false);
                m_animation.state.AddAnimation(0, m_disappearingPlatformData.aboutToDisappearAnimation, true, 0);
            }
        }
        private void OnPlatformReaction(object sender, CollisionEventActionArgs eventArgs)
        {
            if (m_willDisappear == false)
            {
                if (eventArgs.collision.collider.TryGetComponentInParent(out Character character))
                {
                    DisappearPlatform();
                    enabled = true;
                }
            }
        }

        private void Awake()
        {
            m_animation = GetComponentInChildren<SkeletonAnimation>();
            m_collider = GetComponentInChildren<Collider2D>();
            m_hasReactivePlatform = TryGetComponent(out ReactivePlatform platform);
            if (m_hasReactivePlatform)
            {
                platform.OnReaction += OnPlatformReaction;
            }
        }

        void Start()
        {
            m_disappearDelayTimer = m_disappearingPlatformData.disappearDelay;
            if (m_hasReactivePlatform)
            {
                enabled = false;
            }
        }

        void Update()
        {
            if (m_willDisappear == true)
            {
                m_disappearDelayTimer -= Time.deltaTime;
                if (m_disappearDelayTimer <= 0)
                {
                    m_collider.enabled = false;
                    if (m_animation != null)
                    {
                        m_animation.state.SetAnimation(0, m_disappearingPlatformData.disappearAnimation, false);
                        m_animation.state.AddAnimation(0, m_disappearingPlatformData.hiddenAnimation, true, 0.5f);
                    }

                    m_willDisappear = false;
                    m_hasDisappeared = true;
                   
                    m_onDisappear?.Invoke();
                }
            }
           
            else if (m_hasReactivePlatform)
            {
                enabled = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collider)
        {
            //Check whether collider is coming from opposite side of effector
            if (m_hasReactivePlatform == false && collider.enabled)
            {
                DisappearPlatform();
            }
        }

      
    }
}