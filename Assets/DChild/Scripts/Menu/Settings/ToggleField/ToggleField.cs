using DChild.UI;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Menu.UI
{
    public abstract class ToggleField : MonoBehaviour, IValueUI
    {
        protected abstract bool value { get; set; }

        [SerializeField]
        private ToggleButton m_button;

        private bool m_isSettingState;

        public void UpdateUI()
        {
            m_isSettingState = true;
            m_button.SetState(value);
        }

        private void Start()
        {
            m_button.StateUpdate += OnUpdateState;
            m_isSettingState = false;
        }

        private void OnUpdateState(object sender, ButtonToggledEventArgs eventArgs)
        {
            if (m_isSettingState)
            {
                m_isSettingState = false;
            }
            else
            {
                value = eventArgs.isTrue;
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
                if (m_button == null)
                {
                    m_button = GetComponentInChildren<ToggleButton>();
                }
                if (m_button != null)
                {
                    m_instantiated = true;
                }
            }
#endif
        }
    }
}