using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class CelestialIndicator : MonoBehaviour
    {
        [SerializeField,TabGroup("On")]
        private UnityEvent m_onEvents;
        [SerializeField, TabGroup("Off")]
        private UnityEvent m_offEvents;

        private bool m_isOn;
        private bool m_firstInitialization;

        private void OnDrawGizmos()
        {
            
        }
        public void SetState(bool isOn)
        {
            if (m_firstInitialization == false)
            {
                m_isOn = isOn;
            }
            else if (m_isOn != isOn)
            {
                m_isOn = isOn;
                InvokeStateEvents();
            }
        }

        private void InvokeStateEvents()
        {
            if (m_isOn)
            {
                m_onEvents?.Invoke();
            }
            else
            {
                m_offEvents?.Invoke();
            }
        }

        private void Awake()
        {
            if (m_firstInitialization == false)
            {
                InvokeStateEvents();
                m_firstInitialization = true;
            }
        }
    }
}
