using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class ToggableObject : MonoBehaviour
    {
        [SerializeField,HideInPlayMode]
        private bool m_startAs;
        [ShowInInspector,HideInEditorMode]
        private bool m_currentState;

        [SerializeField,TabGroup("True")]
        private UnityEvent m_onTrue;
        [SerializeField, TabGroup("False")]
        private UnityEvent m_onFalse;

        public void SetToggleState(bool value)
        {
            m_currentState = value;
            if (m_currentState)
            {
                m_onTrue?.Invoke();
            }
            else
            {
                m_onTrue?.Invoke();
            }
        }

        private void Start()
        {
            SetToggleState(m_startAs);  
        }
    }
}