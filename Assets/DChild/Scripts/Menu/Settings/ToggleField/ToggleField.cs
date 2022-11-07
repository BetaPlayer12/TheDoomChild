using DChild.UI;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Events;
using Holysoft.Event;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Menu.UI
{
    public abstract class ToggleField : MonoBehaviour, IValueUI
    {
        protected abstract bool value { get; set; }

        [SerializeField]
        private UIToggle m_toggle;

        private bool m_isSettingState;

        public void UpdateUI()
        {
            m_isSettingState = true;
            m_toggle.SetIsOn(value);
        }

        private void Start()
        {
            m_isSettingState = false;
            m_toggle.onToggleValueChangedCallback = OnToggle;
        }

        private void OnToggle(ToggleValueChangedEvent arg0)
        {
            if (m_isSettingState)
            {
                m_isSettingState = false;
            }
            else
            {
                value = arg0.newValue;
            }
        }

#if UNITY_EDITOR
        [SerializeField]
        [HideInInspector]
        private bool m_instantiated;

#endif
        private void OnValidate()
        {
#if UNITY_EDITOR
            if (m_instantiated == false)
            {
                if (m_toggle == null)
                {
                    m_toggle = GetComponentInChildren<UIToggle>();
                }
                if (m_toggle != null)
                {
                    m_instantiated = true;
                }
            }
#endif
        }
    }
}