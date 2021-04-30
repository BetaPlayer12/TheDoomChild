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

        private void Awake()
        {
            m_isToggled = GetComponent<IToggleDebugBehaviour>().value;
        }

        private void Start()
        {
            if (m_isToggled)
            {
                m_highlight.UseHighlightState();
            }
            else
            {
                m_highlight.UseNormalizeState();
            }
        }
    }

}