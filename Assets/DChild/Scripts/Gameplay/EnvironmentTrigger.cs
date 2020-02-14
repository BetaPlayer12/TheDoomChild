using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay
{
    public class EnvironmentTrigger : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData: ISaveData
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

        public ISaveData Save()
        {
            return new SaveData(enabled == false);
        }

        public void Load(ISaveData data)
        {
            enabled = ((SaveData)data).wasTriggered == false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                m_enterEvents?.Invoke();
                if (m_oneTimeOnly)
                {
                    enabled = false;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                m_exitEvents?.Invoke();
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

        [Button,HideInEditorMode]
        private void OnEnter()
        {
            m_enterEvents?.Invoke();
        }

        [Button,HideIf("m_oneTimeOnly"), HideInEditorMode]
        private void OnExit()
        {
            m_exitEvents?.Invoke();
        }
#endif
    }
}