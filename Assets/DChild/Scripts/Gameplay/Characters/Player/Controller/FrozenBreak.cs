using System;
using DChild.Gameplay.Combat.StatusAilment;
using DChild.Inputs;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

    public class FrozenBreak : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField, MinValue(0.1)]
        private float m_spamIncrement;
        [SerializeField, MinValue(0)]
        private float m_breakDecay;

        [SerializeField, ReadOnly]
        private float m_spamValue;
        private float m_breakValue;
        private int m_currentDirectionInput;

        private StatusEffectReciever m_statusEffectReciever;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_statusEffectReciever = info.statusEffectReciever;
            m_statusEffectReciever.StatusEnd += OnStatusEnd;
        }

        private void OnStatusEnd(object sender, StatusEffectRecieverEventArgs eventArgs)
        {
            m_spamValue = 0;
        }

        public void HandleBreak(DirectionalInput input)
        {
            if (input.horizontalInput == m_currentDirectionInput)
            {
                m_spamValue += m_spamIncrement * Time.deltaTime;
                m_currentDirectionInput = m_currentDirectionInput == 1 ? -1 : 1;
                if (m_spamValue >= 100)
                {
                    m_statusEffectReciever.StopStatusEffect(StatusEffectType.Frozen);
                    m_spamValue = 0;
                }
            }
            else if (m_spamValue > 0)
            {
                m_spamValue -= m_breakDecay * Time.deltaTime;
            }
        }

        private void Awake()
        {
            m_breakValue = 100f;
            m_spamValue = 0f;
            m_currentDirectionInput = 1;
        }
    }
}