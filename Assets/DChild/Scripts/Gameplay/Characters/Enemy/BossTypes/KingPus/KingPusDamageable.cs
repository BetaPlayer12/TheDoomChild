using DChild.Gameplay.Characters.Enemies;
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
        private Phase m_currentPhase = Phase.PhaseOne;

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
            m_currentPhase = Phase.PhaseOne;
        }

        private void OnChangePhase(object sender, EventActionArgs eventArgs)
        {
            m_currentPhase++;
            if(m_currentPhase != Phase.PhaseFour)
                RestoreHPForPhaseChange();
        }

        private void RestoreHPForPhaseChange()
        {
            Heal(1000);
        }

        public override void TakeDamage(int totalDamage, DamageType type)
        {
            if(totalDamage <= base.health.currentValue)
            {
                base.TakeDamage(totalDamage, type);
            }
            else if(totalDamage >= base.health.currentValue) 
            {
                if (m_currentPhase == Phase.PhaseThree)
                {
                    Debug.Log("Going to Die!");
                    m_currentPhase = Phase.PhaseFour;
                    PhaseChangeTime?.Invoke(this, new EventActionArgs());
                }
                else
                {
                    Debug.Log("Phase Change Time!");
                    PhaseChangeTime?.Invoke(this, new EventActionArgs());
                }
            }
        }
    }
}

