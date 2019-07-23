﻿using System;
using DChild.Gameplay.Characters;
using Holysoft.Collections;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Combat;
using Spine;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters
{
    public class DeathHandle : MonoBehaviour
    {
        [SerializeField]
        private Damageable m_source;
        [SerializeField]
        private SpineRootAnimation m_animator;
        [SerializeField]
        private CountdownTimer m_bodyDuration;

        [SerializeField]
        private GameObject m_hitCollider;
        [SerializeField]
        private GameObject m_hitBox;

        private string m_animation;

        public void SetAnimation(string animation)
        {
            m_animation = animation;
        }

        private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
        {
            m_source.gameObject.SetActive(false);
            enabled = false;
        }

        private void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_animator.SetAnimation(0, m_animation, false, 0);
            m_animator.animationState.Complete += OnDeathAnimationComplete;
        }

        private void OnDeathAnimationComplete(TrackEntry trackEntry)
        {
            m_hitCollider.SetActive(false);
            m_hitBox.SetActive(false);
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

        private void Update()
        {
            m_bodyDuration.Tick(Time.deltaTime);
        }
    }
}