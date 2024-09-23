using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChildDebug
{
    public class ArmyHealth : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_label;
        [SerializeField]
        private Health m_health;

        private void Start()
        {
            m_health.ValueChanged += OnValueChanged;
            m_label.text = m_health.currentValue.ToString();
        }

        private void OnValueChanged(object sender, StatInfoEventArgs eventArgs)
        {
            m_label.text = eventArgs.currentValue.ToString(); ;
        }
    }

}