using Doozy.Runtime.UIManager.Components;
using Holysoft.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChildDebug.Window
{

    public class DebugToggle : MonoBehaviour
    {
        [SerializeField]
        private UIToggle m_toggle;
        [SerializeField, TabGroup("True")]
        private UnityEvent OnTrue;
        [SerializeField, TabGroup("False")]
        private UnityEvent OnFalse;
        private bool m_isToggled;

        private IToggleDebugBehaviour m_source;

        public void ToggleState()
        {
            m_isToggled = !m_isToggled;
            SetState(m_isToggled);
        }

        public void SetState(bool isOn)
        {
            if (isOn)
            {
                OnTrue?.Invoke();
            }
            else
            {
                OnFalse?.Invoke();
            }
        }

        public void UpdateToggleHighlight()
        {
            m_isToggled = m_source.value;
            m_toggle.SetIsOn(m_isToggled);
        }
        private void Start()
        {
            m_source = GetComponent<IToggleDebugBehaviour>();
            UpdateToggleHighlight();
            m_toggle.OnValueChangedCallback.AddListener(SetState);
        }


    }

}