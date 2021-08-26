using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using System;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class ExplosiveBarrel : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_explosion;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_detonateAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_trapTriggeredAnimation;

        private static FXSpawnHandle<FX> m_fxSpawner;
        private Damageable m_damageable;
        private SpineAnimation m_animation;
        private bool m_hasTriggered;
        private bool m_hasExploded;

        public void TriggerDetonation()
        {
            if (m_hasTriggered == false)
            {
                m_hasTriggered = true;
                StartCoroutine(DetonationRoutine());
            }
        }

        private void Explode()
        {
            if (m_explosion)
            {
                var fx = m_fxSpawner.InstantiateFX(m_explosion, m_damageable.position, gameObject.scene);
                var fxTransform = fx.transform;
                fxTransform.SetParent(m_animation.transform);
                fxTransform.localScale = Vector3.one;
                fxTransform.SetParent(null);
            }
            m_hasExploded = true;
        }

        private void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            m_hasTriggered = true;
            StartCoroutine(AbruptExplosionRoutine());
        }

        private IEnumerator DetonationRoutine()
        {
            m_animation.SetAnimation(0, m_trapTriggeredAnimation, false, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_trapTriggeredAnimation);
            Explode();
            gameObject.SetActive(false);
        }

        private IEnumerator AbruptExplosionRoutine()
        {
            m_animation.SetAnimation(0, m_detonateAnimation, false, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_detonateAnimation);
            Explode();
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            m_damageable = GetComponent<Damageable>();
            m_damageable.Destroyed += OnDestroyed;
            m_animation = GetComponentInChildren<SpineAnimation>();
        }

    }

}