using DChild.Gameplay.Characters;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class DisappearingPlatform : MonoBehaviour
    {
        [SerializeField]
        private DisappearingPlatformData m_disappearingPlatformData;
        [SerializeField, TabGroup("OnDisappear")]
        private UnityEvent m_onDisappear;
        [SerializeField, TabGroup("OnReappear")]
        private UnityEvent m_onReappear;

        private SkeletonAnimation m_animation;
        private Collider2D m_collider;
        private bool m_willDisappear = false;
        private bool m_hasDisappeared = false;
        private float m_disappearDelayTimer;
        private float m_disappearDurationTimer;
        private bool m_hasReactivePlatform;



        private void EnableCollider(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == m_disappearingPlatformData.reappearAnimation)
            {
                m_collider.enabled = true;
                m_disappearDurationTimer = m_disappearingPlatformData.disappearDuration;
                m_disappearDelayTimer = m_disappearingPlatformData.disappearDelay;
                m_animation.state.Interrupt -= EnableCollider;
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
            m_disappearDurationTimer = m_disappearingPlatformData.disappearDuration;
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
                        if (m_disappearingPlatformData.hiddenAnimation != "")
                        {
                            m_animation.state.AddAnimation(0, m_disappearingPlatformData.hiddenAnimation, true, 0.5f);
                        }
                    }

                    m_willDisappear = false;
                    m_hasDisappeared = true;
                    m_disappearDurationTimer = m_disappearingPlatformData.disappearDuration;
                    m_onDisappear?.Invoke();
                }
            }
            else if (m_hasDisappeared == true)
            {
                m_disappearDurationTimer -= Time.deltaTime;
                if (m_disappearDurationTimer <= 0)
                {
                    if (m_animation != null)
                    {
                        m_animation.state.SetAnimation(0, m_disappearingPlatformData.reappearAnimation, false);
                        m_animation.state.AddAnimation(0, m_disappearingPlatformData.idleAnimation, true, 0.8f);

                        m_animation.state.Interrupt += EnableCollider;
                    }
                    m_onReappear?.Invoke();
                    m_hasDisappeared = false;
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
                if (collider.collider.TryGetComponentInParent(out Character character))
                {
                    DisappearPlatform();
                }
            }
        }
    }
}
