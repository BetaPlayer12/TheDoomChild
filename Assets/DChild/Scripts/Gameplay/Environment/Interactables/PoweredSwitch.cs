/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    [AddComponentMenu("DChild/Gameplay/Environment/Interactable/Switches/Powered Switch")]
    public class PoweredSwitch : Switch
    {
        [SerializeField, BoxGroup("Fields")]
        private bool m_isOperational;

        [TabGroup("Main", "Reaction")]
        [SerializeField, TabGroup("Main/Reaction", "Generic")]
        private UnityEvent m_onInteract;
        [SerializeField, TabGroup("Main/Reaction", "NonOperational")]
        private UnityEvent m_onNotOperational;

        public void SetPower(bool isOn) => m_isOperational = isOn;

        public override void Interact()
        {
            m_onInteract?.Invoke();
            if (m_isOperational)
            {
                base.Interact();
            }
            else
            {
                m_onNotOperational?.Invoke();
            }
        }
    }
}