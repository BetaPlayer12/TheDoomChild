using DChild.Gameplay.Combat;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables.Unique
{
    public class SoulLamp : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_breakFX;
        [SerializeField]
        private float m_fixAfterBreakDuration;
        [SerializeField]
        private bool m_fixWhenOffScreen;

        private bool m_isOnScreen;
        private BreakableObject m_breakable;
        private Damageable m_damageable;
        private float m_fixAfterBreakTimer;

        private FXSpawnHandle<FX> m_spawnHandle;

        public void StopFixTimer()
        {
            enabled = false;
        }

        private void OnLampDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_spawnHandle.InstantiateFX(m_breakFX, m_damageable.position);
            m_fixAfterBreakTimer = m_fixAfterBreakDuration;
            enabled = true;
        }

        private void Start()
        {
            m_breakable = GetComponent<BreakableObject>();
            m_damageable = GetComponent<Damageable>();

            m_damageable.Destroyed += OnLampDestroyed;
            enabled = false;
        }

        private void OnBecameInvisible()
        {
            m_isOnScreen = false;
        }

        private void OnBecameVisible()
        {
            m_isOnScreen = true;
        }

        private void LateUpdate()
        {
            if (m_fixAfterBreakTimer > 0)
            {
                m_fixAfterBreakTimer -= GameplaySystem.time.deltaTime;
            }
            else
            {
                if (m_fixWhenOffScreen && m_isOnScreen)
                {
                    return;
                }

                m_breakable.SetObjectState(false);
                enabled = false;
            }
        }

    }

}