using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class BaseToggableObject : ToggableObject
    {
        [SerializeField, TabGroup("True")]
        private UnityEvent m_onTrue;
        [SerializeField, TabGroup("False")]
        private UnityEvent m_onFalse;

        private bool m_hasBeenInitialize;

        public override  void SetToggleState(bool value)
        {
            base.SetToggleState(value);
            if (m_currentState)
            {
                m_onTrue?.Invoke();
            }
            else
            {
                m_onFalse?.Invoke();
            }
        }

        private void Start()
        {
            if (m_hasBeenInitialize == false)
            {
                SetToggleState(m_startAs);
                m_hasBeenInitialize = true;
            }
        }
    }
}