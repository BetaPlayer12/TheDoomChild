using System;
using DChild.Gameplay.Characters;
using Holysoft.Collections;
using Holysoft.Event;
using DChild.Gameplay.Combat;
using Spine;
using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters
{
    public class DeathHandle : MonoBehaviour, IHasSkeletonDataAsset
    {
        public struct DisposingEventArgs : IEventActionArgs
        {
            public DisposingEventArgs(bool isBodyDestroyed)
            {
                this.isBodyDestroyed = isBodyDestroyed;
            }

            public bool isBodyDestroyed { get; }
        }

        [SerializeField]
        private Damageable m_source;
        [SerializeField]
        private SpineRootAnimation m_animator;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_animation;
        [SerializeField]
        private CountdownTimer m_bodyDuration;
        [SerializeField]
        private List<ParticleSystem> m_fx;
        [SerializeField]
        private bool m_destroySource;

        public event EventAction<DisposingEventArgs> BodyDestroyed;
        SkeletonDataAsset IHasSkeletonDataAsset.SkeletonDataAsset => m_animator.GetComponentInChildren<SkeletonAnimation>().skeletonDataAsset;

        public void SetDestroySource(bool value) => m_destroySource = value;

        public void SetAnimation(string animation)
        {
            m_animation = animation;
        }

        private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_destroySource)
            {
                BodyDestroyed?.Invoke(this, new DisposingEventArgs(true));
                Destroy(m_source.gameObject);
            }
            else
            {
                BodyDestroyed?.Invoke(this, new DisposingEventArgs(false));
                m_source.gameObject.SetActive(false);
                enabled = false;
            }
        }

        private void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            if (m_animator == null)
            {
                m_bodyDuration.Reset();
                enabled = true;
            }
            else
            {
                m_source.SetHitboxActive(false);
                m_animator.SetAnimation(0, m_animation, false, 0);
                m_animator.animationState.Complete += OnDeathAnimationComplete;
            }

            for (int i = 0; i < m_fx.Count; i++)
            {
                m_fx[i].Stop();
            }
        }

        private void OnDeathAnimationComplete(TrackEntry trackEntry)
        {
            m_bodyDuration.Reset();
            m_animator.animationState.Complete -= OnDeathAnimationComplete;
            enabled = true;
        }

        private void Awake()
        {
            m_source.Destroyed += OnDestroyed;
            m_bodyDuration.CountdownEnd += OnCountdownEnd;
            enabled = false;
        }

        private void OnDisable()
        {
            m_bodyDuration.Reset();
            enabled = false;
        }

        private void Update()
        {
            m_bodyDuration.Tick(Time.deltaTime);
        }

#if UNITY_EDITOR
        public void InitializeField(Damageable damageable, SpineRootAnimation spineRoot)
        {
            m_source = damageable;
            m_animator = spineRoot;
        }
#endif
    }
}