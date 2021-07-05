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
        private UIHighlight m_highlight;
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
                m_highlight.Highlight();
                OnTrue?.Invoke();
            }
            else
            {
                m_highlight.Normalize();
                OnFalse?.Invoke();
            }
        }

        public void UpdateToggleHighlight()
        {
            m_isToggled = m_source.value;
            if (m_isToggled)
            {
                m_highlight.UseHighlightState();
            }
            else
            {
                m_highlight.UseNormalizeState();
            }

        }
        private void Start()
        {
            m_source = GetComponent<IToggleDebugBehaviour>();
            UpdateToggleHighlight();
        }


    }

}