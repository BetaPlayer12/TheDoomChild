using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{

    public class PhaseHandle<T, U> where T : System.Enum
                                  where U : IPhaseInfo
    {
        [SerializeField, OnValueChanged("PhaseChanged"), EnableIf("m_overidePhase")]
        private T m_currentPhase;
#if UNITY_EDITOR
        [ShowInInspector]
        private bool m_overidePhase;
#endif
        private PhaseInfo<T> m_phaseInfo;
        private IPhaseConditionHandle<T> m_conditionHandle;
        private bool m_willTransistion;
        private bool m_isTransistioning;
        private Action ChangeState;
        private Action<U> ApplyPhaseData;

        public bool allowPhaseChange;
        public T currentPhase { get => m_currentPhase; }

        public void Initialize(T startingPhase, PhaseInfo<T> phaseInfo, Character character, Action ChangeStateFunction, Action<U> ApplyPhaseDataFunction)
        {
            this.m_currentPhase = startingPhase;
            m_phaseInfo = phaseInfo;
            m_conditionHandle = m_phaseInfo.CreateConditionHandle(character);
            ChangeState = ChangeStateFunction;
            ApplyPhaseData = ApplyPhaseDataFunction;
            m_willTransistion = false;
            allowPhaseChange = true;
#if UNITY_EDITOR
            m_overidePhase = false;
#endif
        }

        public void SetPhase(T phase)
        {
            m_currentPhase = phase;
            m_willTransistion = true;
            if (allowPhaseChange)
            {
                ChangeState();
                m_isTransistioning = true;
            }
        }

        public void MonitorPhase()
        {
#if UNITY_EDITOR
            if (m_overidePhase == false)
            {
#endif
                if (m_willTransistion == false)
                {
                    var phase = m_conditionHandle.GetProposedPhase();
                    if (m_currentPhase.Equals(phase) == false)
                    {
                        SetPhase(phase);
                    }
                }
                else if (m_isTransistioning == false && allowPhaseChange)
                {
                    ChangeState();
                    m_isTransistioning = true;
                }
#if UNITY_EDITOR
            }
#endif
        }

        public void ApplyChange()
        {
            ApplyPhaseData((U)m_phaseInfo.GetDataOfPhase(m_currentPhase));
            m_willTransistion = false;
            m_isTransistioning = false;
        }

        private void PhaseChanged()
        {
            ChangeState();
            m_willTransistion = true;
        }
    }
}