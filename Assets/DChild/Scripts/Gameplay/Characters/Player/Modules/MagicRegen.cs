using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class MagicRegen : MonoBehaviour, IPlayerExternalModule
    {
        [SerializeField]
        private CountdownTimer m_regenInterval = new CountdownTimer(5);
        [SerializeField]
        [Range(0f, 1f)]
        private float m_regenRate;
        private ICappedStat m_magic;
        private bool m_isFull;
        private IIsolatedTime m_time;

        public void Initialize(IPlayerModules player)
        {
            m_time = player.isolatedObject;
            m_magic = player.magic;
            m_isFull = m_magic.currentValue == m_magic.maxValue;
            m_magic.ValueChanged += OnValueChange;
        }

        public void SetRegenRate(float regenRate)
        {
            m_regenRate = regenRate;
        }

        private void OnValueChange(object sender, StatInfoEventArgs eventArgs)
        {
            m_isFull = m_magic.currentValue == m_magic.maxValue;
            if (m_isFull)
            {
                enabled = false;
                m_regenInterval.Reset();
            }
            else
            {
                enabled = true;
            }
        }

        private void OnIntervalEnd(object sender, EventActionArgs eventArgs)
        {
            m_regenInterval.Reset();
            m_magic.AddCurrentValue(Mathf.CeilToInt(m_magic.maxValue * m_regenRate));
            m_isFull = m_magic.currentValue == m_magic.maxValue;
            if (m_isFull)
            {
                enabled = false;
            }
        }

        private void Awake()
        {
            m_regenInterval.CountdownEnd += OnIntervalEnd;
            m_regenInterval.Reset();
            enabled = false;
        }

        private void Update()
        {
            m_regenInterval.Tick(m_time.deltaTime);
        }
    }

}