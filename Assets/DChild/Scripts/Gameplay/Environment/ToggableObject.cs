using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class ToggableObject : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_currentState;

            public SaveData(bool currentState)
            {
                m_currentState = currentState;
            }

            public bool currentState => m_currentState;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_currentState);
        }

        [SerializeField, HideInPlayMode]
        private bool m_startAs;
        [ShowInInspector, HideInEditorMode, OnValueChanged("ToggleState")]
        private bool m_currentState;

        [SerializeField, TabGroup("True")]
        private UnityEvent m_onTrue;
        [SerializeField, TabGroup("False")]
        private UnityEvent m_onFalse;

        private bool m_hasBeenInitialize;

        public void Load(ISaveData data)
        {
            SetToggleState(((SaveData)data).currentState);
            m_hasBeenInitialize = true;
        }

        public ISaveData Save() => new SaveData(m_currentState);

        [Button,HideInEditorMode]
        public void ToggleState()
        {
            SetToggleState(!m_currentState);
        }

        public void SetToggleState(bool value)
        {
            m_currentState = value;
            if (m_currentState)
            {
                m_onTrue?.Invoke();
            }
            else
            {
                m_onFalse?.Invoke();
            }
            m_hasBeenInitialize = true;
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