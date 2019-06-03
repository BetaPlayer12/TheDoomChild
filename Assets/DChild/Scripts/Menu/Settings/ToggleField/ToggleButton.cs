using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Menu.UI
{
    public struct ButtonToggledEventArgs : IEventActionArgs
    {
        public ButtonToggledEventArgs(bool isTrue) : this()
        {
            this.isTrue = isTrue;
        }

        public bool isTrue { get; }
    }

    public sealed class ToggleButton : MonoBehaviour
    {
        public event EventAction<ButtonToggledEventArgs> StateUpdate;

        [SerializeField]
        private bool m_isTrue;

        public void SetState(bool isTrue)
        {
            m_isTrue = isTrue;
            StateUpdate?.Invoke(this, new ButtonToggledEventArgs(m_isTrue));
        }

        public void Toggle()
        {
            m_isTrue = !m_isTrue;
            StateUpdate?.Invoke(this, new ButtonToggledEventArgs(m_isTrue));
        }

        private void OnValidate()
        {
            StateUpdate?.Invoke(this, new ButtonToggledEventArgs(m_isTrue));
        }
    }
}