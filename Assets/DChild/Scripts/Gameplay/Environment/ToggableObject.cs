using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public abstract class ToggableObject : MonoBehaviour, ISerializableComponent
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
        protected bool m_startAs;
        [ShowInInspector, HideInEditorMode, OnValueChanged("ToggleState")]
        protected bool m_currentState;

        protected bool m_hasBeenInitialize;

        public void Load(ISaveData data)
        {
            SetToggleState(((SaveData)data).currentState);
            m_hasBeenInitialize = true;
        }

        public ISaveData Save() => new SaveData(m_currentState);

        [Button, HideInEditorMode]
        public void ToggleState()
        {
            SetToggleState(!m_currentState);
        }

        public virtual void SetToggleState(bool value)
        {
            m_currentState = value;
            m_hasBeenInitialize = true;
        }
    }
}