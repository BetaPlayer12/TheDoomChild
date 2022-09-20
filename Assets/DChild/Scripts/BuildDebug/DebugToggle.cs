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
        [SerializeField, TabGroup("True")]
        private UnityEvent OnTrue;
        [SerializeField, TabGroup("False")]
        private UnityEvent OnFalse;
        private bool m_isToggled;

        private IToggleDebugBehaviour m_source;

        public void ToggleState()
        {
            m_isToggled = !m_isToggled;
            if (m_isToggled)
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
        }
        private void Start()
        {
            m_source = GetComponent<IToggleDebugBehaviour>();
            UpdateToggleHighlight();
        }


    }

}