using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ReviveBlob : MonoBehaviour
    {
        [SerializeField]
        private Damageable m_damageable;
        [SerializeField]
        private Hitbox m_hitbox;
        [SerializeField]
        private Health m_health;
        [SerializeField]
        private SpineRootAnimation m_animator;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_recoverAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_idleAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_deathAnimation;
        [SerializeField, MinValue(0)]
        private float m_deathDuration;

        private void Awake()
        {
            m_damageable.Destroyed += OnBlobDie;
        }

        private void OnBlobDie(object sender, EventActionArgs eventArgs)
        {
            StartCoroutine(ReviveBlobRoutine());
        }

        private IEnumerator ReviveBlobRoutine()
        {
            m_hitbox.Disable();
            m_animator.SetAnimation(0, m_deathAnimation, false);
            yield return new WaitForAnimationComplete(m_animator.animationState, m_deathAnimation);
            yield return new WaitForSeconds(m_deathDuration);
            m_animator.SetAnimation(0, m_recoverAnimation, false);
            yield return new WaitForAnimationComplete(m_animator.animationState, m_recoverAnimation);
            m_hitbox.Enable();
            enabled = true;
            m_health.SetHealthPercentage(1f);
            m_animator.SetAnimation(0, m_idleAnimation, true);
        }
    }

}
