using DChild.Gameplay.UI;
using Holysoft;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat.UI
{
    public class CappedStatUI : MonoBehaviour
    {
        [SerializeField]
        private StatUI m_ui;
        private ICappedStat m_stat;

        public void MonitorInfoOf(ICappedStat stat)
        {
            if(m_stat != null)
            {
                m_stat.ValueChanged -= OnValueChange;
                m_stat.MaxValueChanged -= OnMaxValueChange;
            }
            m_stat = stat;
            if (m_stat != null)
            {
                m_stat.ValueChanged += OnValueChange;
                m_stat.MaxValueChanged += OnMaxValueChange;
                m_ui.maxValue = m_stat.maxValue;
                m_ui.currentValue = m_stat.currentValue;
            }
            else
            {
                m_ui.maxValue = 0;
                m_ui.currentValue = 0;
            }
        }

        private void OnMaxValueChange(object sender, StatInfoEventArgs eventArgs) => m_ui.maxValue = eventArgs.maxValue;

        private void OnValueChange(object sender, StatInfoEventArgs eventArgs) => m_ui.currentValue = eventArgs.currentValue;

        private void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this, ref m_ui, ComponentUtility.ComponentSearchMethod.Child);
        }
    }

}