using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay
{
    public class EnvironmentTrigger : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            public SaveData(bool wasTriggered)
            {
                this.wasTriggered = wasTriggered;
            }

            public bool wasTriggered { get; }
        }

        [SerializeField, OnValueChanged("OnValueChange")]
        private bool m_oneTimeOnly;
        [SerializeField]
        private UnityEvent m_enterEvents;
        [SerializeField, HideIf("m_oneTimeOnly")]
        private UnityEvent m_exitEvents;

        private bool m_wasTriggered;

        public ISaveData Save()
        {
            return new SaveData(m_wasTriggered);
        }

        public void Load(ISaveData data)
        {
            m_wasTriggered = ((SaveData)data).wasTriggered;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                if ((m_oneTimeOnly && !m_wasTriggered) || !m_oneTimeOnly)
                {
                    m_enterEvents?.Invoke();
                }
                if (m_oneTimeOnly)
                {
                    m_wasTriggered = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_oneTimeOnly == false)
            {
                if (collision.CompareTag("Hitbox"))
                {
                    m_exitEvents?.Invoke();
                }
            }
        }

#if UNITY_EDITOR
        private void OnValueChange()
        {
            if (m_oneTimeOnly)
            {
                m_exitEvents.RemoveAllListeners();
            }
        }

        [Button, HideInEditorMode]
        private void OnEnter()
        {
            m_enterEvents?.Invoke();
        }

        [Button, HideIf("m_oneTimeOnly"), HideInEditorMode]
        private void OnExit()
        {
            m_exitEvents?.Invoke();
        }
#endif
    }
}