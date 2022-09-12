using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Systems
{

    public class ShadowEnvironmentHandle : MonoBehaviour
    {
        [SerializeField, InfoBox("These colliders will be ignored by the player when he is in SHADOW mode"), TabGroup("ShadowColliders")]
        private Collider2D[] m_shadowColliders;
        [SerializeField, InfoBox("These colliders will be ignored by the player when he is NOT in SHADOW mode"), TabGroup("NonShadowColliders")]
        private Collider2D[] m_reverseShadowColliders;
        [SerializeField, InfoBox("These colliders will be used to trigger events by the player when he is in SHADOW mode"), TabGroup("EventShadowColliders")]
        private Collider2D[] m_eventShadowColliders;
        [SerializeField, TabGroup("ShadowOnEvents")]
        private UnityEvent m_OnEvents;
        [SerializeField, TabGroup("ShadowOffEvents")]
        private UnityEvent m_OffEvents;
        private Collider2D[] m_playerColliders;

        private bool m_isInShadowEnvironment;

        public void SetCollisions(bool enableCollisions)
        {
            if (m_isInShadowEnvironment != enableCollisions)
            {
                SetIgnoredInShadowModeCollisionState(enableCollisions);
                SetEnableInShadowModeCollisionState(enableCollisions);
                ActivateEventsInShadowModeCollisionState(enableCollisions);
                m_isInShadowEnvironment = enableCollisions;
            }
        }

        private void SetIgnoredInShadowModeCollisionState(bool enableCollisions)
        {
            if (m_shadowColliders.Length > 0)
            {
                for (int i = 0; i < m_playerColliders.Length; i++)
                {
                    for (int j = 0; j < m_shadowColliders.Length; j++)
                    {
                        try
                        {
                            Physics2D.IgnoreCollision(m_playerColliders[i], m_shadowColliders[j], enableCollisions);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"Shadow Environment Error Null Reference \n {e.Message}", this);
                        }
                    }
                }
            }
        }
        private void SetEnableInShadowModeCollisionState(bool enableCollisions)
        {
            if (m_reverseShadowColliders.Length > 0)
            {
                var ignoreCollision = !enableCollisions;
                for (int i = 0; i < m_playerColliders.Length; i++)
                {
                    for (int j = 0; j < m_reverseShadowColliders.Length; j++)
                    {
                        try
                        {
                            Physics2D.IgnoreCollision(m_playerColliders[i], m_reverseShadowColliders[j], ignoreCollision);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"Shadow Environment Error Null Reference \n {e.Message}", this);
                        }
                    }
                }
            }
        }
        private void ActivateEventsInShadowModeCollisionState(bool enableCollisions)
        {
            if (enableCollisions == true)
            {
                Debug.Log("On");
                m_OnEvents?.Invoke();

            }
            else
            {
                Debug.Log("Off");
                m_OffEvents.Invoke();
            }
        }
        private void Start()
        {
            m_playerColliders = GameplaySystem.playerManager.player.character.colliders.colliders;
            GameplaySystem.world.Register(this);

            //Force Set Collision
            m_isInShadowEnvironment = true;
            SetCollisions(false);
        }
    }
}