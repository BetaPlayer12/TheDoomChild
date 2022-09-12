using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class ToggleEventHandle : MonoBehaviour
    {

        [TabGroup("Main", "StartAs")]
        [SerializeField, TabGroup("Main/StartAs", "ToggleOn")]
        private UnityEvent m_OnStartEvent;
        [SerializeField, TabGroup("Main/StartAs", "ToggleOff")]
        private UnityEvent m_OffStartEvent;
        [TabGroup("Main", "Transition")]
        [SerializeField, TabGroup("Main/Transition","ToggleOn")]
        private UnityEvent m_OnTransitionEvent;
        [SerializeField, TabGroup("Main/Transition", "ToggleOff")]
        private UnityEvent m_OffTransitionEvent;

        public void ToggleOnStart(bool isOn)
        {
            if (isOn)
            {
                m_OnStartEvent?.Invoke();
            }
            else
            {
                m_OffStartEvent?.Invoke();
            }

        }
        public void ToggleOnTransition(bool isOn)
        {
            if (isOn)
            {
                m_OnTransitionEvent?.Invoke();
            }
            else
            {
                m_OffTransitionEvent?.Invoke();
            }
            
        }
    }
}
