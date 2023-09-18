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
    public class OrangeBlob : MonoBehaviour
    {
        [SerializeField]
        private Damageable m_damageable;
        [SerializeField]
        private Hitbox m_hitbox;
        [SerializeField]
        private Collider2D m_hitboxCollider;
        [SerializeField]
        private Health m_health;
        [SerializeField]
        private SpineRootAnimation m_animator;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_recoverAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_idleAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_idleAnimationTwo;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_deathAnimation;
        [SerializeField]
        private ParticleSystem m_deathVFX;
        [SerializeField, MinValue(0)]
        private float m_deathDuration;
        [SerializeField]
        private Collider2D m_itemlocked;

        private void Awake()
        {
            if (m_itemlocked != null)
            {
                m_itemlocked.enabled = false;
            }
            m_damageable.Destroyed += OnBlobDie;
        }

        private void Start()
        {
            SetRandomIdleAnimation();
        }

        private void SetRandomIdleAnimation()
        {
            int randomNumber = UnityEngine.Random.Range(1, 3);

            if (randomNumber == 1)
            {
                m_animator.SetAnimation(0, m_idleAnimation, true);
            }
            else
            {
                m_animator.SetAnimation(0, m_idleAnimationTwo, true);
            }
        }

        private void OnBlobDie(object sender, EventActionArgs eventArgs)
        {
            if (m_itemlocked != null)
            {
                m_itemlocked.enabled = true;
            }
            StartCoroutine(ReviveBlobRoutine());
        }

        private IEnumerator ReviveBlobRoutine()
        {
            m_hitbox.Disable();
            m_hitboxCollider.enabled = false;
            m_deathVFX.Play();
            m_animator.SetAnimation(0, m_deathAnimation, false);
            yield return new WaitForAnimationComplete(m_animator.animationState, m_deathAnimation);
            yield return new WaitForSeconds(m_deathDuration);
            m_animator.SetAnimation(0, m_recoverAnimation, false);
            yield return new WaitForAnimationComplete(m_animator.animationState, m_recoverAnimation);
            SetRandomIdleAnimation();
            m_hitbox.Enable();
            m_hitboxCollider.enabled = true;
            enabled = true;
            m_health.SetHealthPercentage(1f);
            if (m_itemlocked != null)
            {
                m_itemlocked.enabled = false;
            }
        }
    }

}
