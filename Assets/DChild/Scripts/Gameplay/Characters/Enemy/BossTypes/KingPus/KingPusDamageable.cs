using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.UI;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class KingPusDamageable : Damageable
    {
        [SerializeField]
        private KingPusAI m_kingPusAI;
        [SerializeField]
        private KingPusUIHandle m_kingPusUIHandle;
        [SerializeField]
        private Phase m_currentPhase = Phase.PhaseOne;
        [SerializeField]
        private float m_healDelay;

        public event EventAction<EventActionArgs> PhaseChangeTime;

        enum Phase
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            PhaseFour,
        }

        private void Start()
        {
            PhaseChangeTime += OnChangePhase;
            m_kingPusUIHandle.HideHealthUI += OnHealthHide;
            m_currentPhase = Phase.PhaseOne;
        }

        private void OnHealthHide(object sender, EventActionArgs eventArgs)
        {
            if (m_currentPhase != Phase.PhaseFour)
                StartCoroutine(DelayBeforeHeal(m_healDelay));
        }

        private void OnChangePhase(object sender, EventActionArgs eventArgs)
        {
            if (m_currentPhase < Phase.PhaseFour)
            {
                m_currentPhase++;
            }
        }

        private void RestoreHPForPhaseChange()
        {
            Heal(9999);
        }

        private IEnumerator DelayBeforeHeal(float delay)
        {
            yield return new WaitForSeconds(delay);
            RestoreHPForPhaseChange();
        }

        public override void TakeDamage(int totalDamage, DamageType type)
        {
            if (totalDamage < base.health.currentValue)
            {
                base.TakeDamage(totalDamage, type);
            }
            else if (totalDamage >= base.health.currentValue)
            {
                if (m_currentPhase == Phase.PhaseThree)
                {
                    Debug.Log("Going to Die!");
                    m_currentPhase = Phase.PhaseFour;
                    PhaseChangeTime?.Invoke(this, new EventActionArgs());
                }
                else if (m_currentPhase < Phase.PhaseThree)
                {
                    Debug.Log("Phase Change Time!");
                    PhaseChangeTime?.Invoke(this, new EventActionArgs());
                    if (m_kingPusUIHandle == null)
                    {
                        RestoreHPForPhaseChange();
                    }
                }
                else if (m_currentPhase == Phase.PhaseFour)
                {
                    CallDestroyed();
                }
            }
        }
    }
}

