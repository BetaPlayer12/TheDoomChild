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
        [SerializeField]
        private T m_currentPhase;
        private PhaseInfo<T> m_phaseInfo;
        private IPhaseConditionHandle<T> m_conditionHandle;
        private bool willTransistion;
        private Action ChangeState;
        private Action<U> ApplyPhaseData;
        public T currentPhase { get => m_currentPhase; }

        public void Initialize(T startingPhase, PhaseInfo<T> phaseInfo, Character character, Action ChangeStateFunction, Action<U> ApplyPhaseDataFunction)
        {
            this.m_currentPhase = startingPhase;
            m_phaseInfo = phaseInfo;
            m_conditionHandle = m_phaseInfo.CreateConditionHandle(character);
            ChangeState = ChangeStateFunction;
            ApplyPhaseData = ApplyPhaseDataFunction;
            willTransistion = false;
        }

        public void MonitorPhase()
        {
            if (willTransistion == false)
            {
                var phase = m_conditionHandle.GetProposedPhase();
                if (m_currentPhase.Equals(phase) == false)
                {
                    m_currentPhase = phase;
                    willTransistion = true;
                    ChangeState();
                }
            }
        }

        public void ApplyChange()
        {
            ApplyPhaseData((U)m_phaseInfo.GetDataOfPhase(m_currentPhase));
            willTransistion = false;
        }
    }
}