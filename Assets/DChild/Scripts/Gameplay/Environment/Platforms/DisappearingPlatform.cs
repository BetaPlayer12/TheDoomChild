using DChild.Gameplay.Characters;
using Holysoft.Event;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class DisappearingPlatform : MonoBehaviour
    {
        [SerializeField]
        private float m_disappearDelay;
        [SerializeField]
        private float m_disappearDuration;

        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_idleAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_steppedOnAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_aboutToDisappearAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_disappearAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_hiddenAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_reappearAnimation;

        private SkeletonAnimation m_animation;
        private Collider2D m_collider;
        private bool m_willDisappear = false;
        private bool m_hasDisappeared = false;
        private float m_disappearDelayTimer;
        private float m_disappearDurationTimer;
        private bool m_hasReactivePlatform;

        private void EnableCollider(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == m_reappearAnimation)
            {
                m_collider.enabled = true;
                m_disappearDurationTimer = m_disappearDuration;
                m_disappearDelayTimer = m_disappearDelay;
                m_animation.state.Interrupt -= EnableCollider;
            }
        }

        private void DisappearPlatform()
        {
            m_willDisappear = true;
            m_animation.state.SetAnimation(0, m_steppedOnAnimation, false);
            m_animation.state.AddAnimation(0, m_aboutToDisappearAnimation, true, 0);
        }
        private void OnPlatformReaction(object sender, EventActionArgs eventArgs)
        {
            if (m_willDisappear == false)
            {
                DisappearPlatform();
                enabled = true;
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
            m_disappearDelayTimer = m_disappearDelay;
            m_disappearDurationTimer = m_disappearDuration;
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
                    m_animation.state.SetAnimation(0, m_disappearAnimation, false);
                    m_animation.state.AddAnimation(0, m_hiddenAnimation, true, 0);

                    m_willDisappear = false;
                    m_hasDisappeared = true;
                    m_disappearDurationTimer = m_disappearDuration;
                }
            }
            else if (m_hasDisappeared == true)
            {
                m_disappearDurationTimer -= Time.deltaTime;
                if (m_disappearDurationTimer <= 0)
                {
                    m_animation.state.SetAnimation(0, m_reappearAnimation, false);
                    m_animation.state.AddAnimation(0, m_idleAnimation, true, 0);

                    m_animation.state.Interrupt += EnableCollider;
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
                DisappearPlatform();
            }
        }
    }
}
