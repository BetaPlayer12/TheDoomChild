using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.UI
{
    public class PlayerStatUI : SerializedMonoBehaviour
    {
        [SerializeField, BoxGroup("Health")]
        private ICappedStat m_healthStat;
        [SerializeField, BoxGroup("Health")]
        private Image m_healthGlow;

        [SerializeField, BoxGroup("ShadowGauge")]
        private ICappedStat m_shadowGaugeStat;
        [SerializeField, BoxGroup("ShadowGauge")]
        private Image m_consumeGlow;
        [SerializeField, BoxGroup("ShadowGauge"), MinValue(0)]
        private float m_glowDisappearDelay;

        private int m_prevShadowStatValue;
        private bool m_shadowWasConsumed;
        private Coroutine m_shadowConsumedRoutine;

        private void HealthStatChange(object sender, StatInfoEventArgs eventArgs)
        {
            m_healthGlow.enabled = eventArgs.maxValue == eventArgs.currentValue;
        }

        private void ShadowStatChange(object sender, StatInfoEventArgs eventArgs)
        {
            var decreasedSensed = m_shadowGaugeStat.currentValue < m_prevShadowStatValue;
            m_consumeGlow.enabled = decreasedSensed;
            m_shadowWasConsumed = decreasedSensed;
            m_prevShadowStatValue = m_shadowGaugeStat.currentValue;
            if (decreasedSensed)
            {
                if (m_shadowConsumedRoutine == null)
                {
                    m_shadowConsumedRoutine = StartCoroutine(DelayConsumeDisapear());
                }
            }
            else
            {
                if (m_shadowConsumedRoutine != null)
                {
                    StopCoroutine(m_shadowConsumedRoutine);
                }
            }
        }

        private IEnumerator DelayConsumeDisapear()
        {
            var timer = m_glowDisappearDelay;
            while (timer > 0)
            {
                if (m_shadowWasConsumed)
                {
                    timer = m_glowDisappearDelay;
                    m_shadowWasConsumed = false;
                }
                else
                {
                    timer -= GameplaySystem.time.deltaTime;
                }
                yield return null;
            }
            m_consumeGlow.enabled = false;
            m_shadowConsumedRoutine = null;
        }

        private void Awake()
        {
            m_healthStat.MaxValueChanged += HealthStatChange;
            m_healthStat.ValueChanged += HealthStatChange;
            m_healthGlow.enabled = m_healthStat.maxValue == m_healthStat.currentValue;

            m_shadowGaugeStat.ValueChanged += ShadowStatChange;
            m_prevShadowStatValue = m_shadowGaugeStat.currentValue;
        }
    }
}