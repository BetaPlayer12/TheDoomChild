using DChild.Gameplay.Characters.Players.Modules;
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

        [SerializeField, BoxGroup("Shadow Gauge")]
        private ICappedStat m_shadowGauge;
        [SerializeField, BoxGroup("Shadow Gauge")]
        private Canvas m_shadowGaugeFX;
        private int m_previousShadowGauge;

        [SerializeField]
        private StylizedPlayerUI[] m_stylizedUI;

        private CharacterState m_playerState;

        private void HealthStatChange(object sender, StatInfoEventArgs eventArgs)
        {
            m_healthGlow.enabled = eventArgs.maxValue == eventArgs.currentValue;
        }

        private void ShadowGaugeStatChange(object sender, StatInfoEventArgs eventArgs)
        {
            if (m_previousShadowGauge > eventArgs.currentValue)
            {
                StopCoroutine("ShadowGaugeFXRoutine");
                StartCoroutine(ShadowGaugeFXRoutine());
            }
            m_previousShadowGauge = eventArgs.currentValue;
        }

        private IEnumerator ShadowGaugeFXRoutine()
        {
            m_shadowGaugeFX.enabled = true;
            yield return new WaitForSeconds(0.5f);
            m_shadowGaugeFX.enabled = false;
        }

        private void Awake()
        {
            m_healthStat.MaxValueChanged += HealthStatChange;
            m_healthStat.ValueChanged += HealthStatChange;
            m_healthGlow.enabled = m_healthStat.maxValue == m_healthStat.currentValue;

            m_shadowGauge.MaxValueChanged += ShadowGaugeStatChange;
            m_shadowGauge.ValueChanged += ShadowGaugeStatChange;
            m_previousShadowGauge = m_shadowGauge.currentValue;
            m_shadowGaugeFX.enabled = false;
        }

        private void Start()
        {
            m_playerState = GameplaySystem.playerManager.player.state;
        }

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(1), HideInEditorMode]
        private PlayerUIStyle m_changeInto;

        [Button, PropertyOrder(1), HideInEditorMode]
        public void Change()
        {
            for (int i = 0; i < m_stylizedUI.Length; i++)
            {
                m_stylizedUI[i].ChangeTo(m_changeInto);
            }
        }
#endif
    }
}