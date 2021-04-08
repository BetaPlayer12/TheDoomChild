using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ShadowGaugeRegen : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField, MinValue(0)]
        private float m_delayBeforeRegenStart;
        [SerializeField, MinValue(0)]
        private float m_regenRate;

        private ICappedStat m_shadowGauge;
        private float m_delayTimer;
        private float m_stackingRegen;
        private bool m_enableDelayedRegen;
        private int m_previousShadowGauge;
        private bool m_isEnabled;

        private bool shadowGaugeHasBeenReduced => m_previousShadowGauge > m_shadowGauge.currentValue;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_shadowGauge = info.magic;
            m_previousShadowGauge = m_shadowGauge.currentValue;
        }

        public void Enable(bool isEnabled)
        {
            m_isEnabled = isEnabled;
        }

        public bool CanRegen() => m_isEnabled && m_shadowGauge.currentValue < m_shadowGauge.maxValue;

        public void Execute()
        {
            var deltaTime = GameplaySystem.time.deltaTime;
            if (m_enableDelayedRegen)
            {
                if (shadowGaugeHasBeenReduced)
                {
                    m_delayTimer = m_delayBeforeRegenStart;
                    m_stackingRegen = 0;
                }
                else
                {
                    m_delayTimer -= deltaTime;
                    if (m_delayTimer <= 0)
                    {
                        m_enableDelayedRegen = false;
                    }
                }
            }
            else if (shadowGaugeHasBeenReduced)
            {
                m_delayTimer = m_delayBeforeRegenStart;
                m_enableDelayedRegen = true;
                m_stackingRegen = 0;
            }
            else
            {
                RegenerateGauge(deltaTime);
            }
            m_previousShadowGauge = m_shadowGauge.currentValue;
        }

        private void RegenerateGauge(float deltaTime)
        {
            m_stackingRegen += m_regenRate * deltaTime;
            if (m_stackingRegen >= 1)
            {
                var integer = Mathf.FloorToInt(m_stackingRegen);
                m_stackingRegen -= integer;
                m_shadowGauge.AddCurrentValue(integer);
            }
        }
    }

}