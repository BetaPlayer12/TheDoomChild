using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public abstract class CriticalStatHandle : SerializedMonoBehaviour
    {
        [SerializeField]
        private ICappedStat m_stat;
        [SerializeField, MinValue(1)]
        private int m_valueThreshold;

        private void OnStatValueChanged(object sender, StatInfoEventArgs eventArgs)
        {
            if (eventArgs.currentValue <= m_valueThreshold)
            {
                OnStatAtCriticalValue();
            }
            else
            {
                CancelCriticalEffects();
            }
        }

        protected abstract void CancelCriticalEffects();

        protected abstract void OnStatAtCriticalValue();

        private void Awake()
        {
            m_stat.ValueChanged += OnStatValueChanged;
        }
    }
}