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
                this.m_isTriggered = wasTriggered;
            }

            [ShowInInspector]
            public bool m_isTriggered;
            public bool isTriggered => m_isTriggered;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isTriggered);
        }

        [SerializeField, OnValueChanged("OnValueChange")]
        private bool m_oneTimeOnly;
        [SerializeField, TabGroup("Enter")]
        private UnityEvent m_enterEvents;
        [SerializeField, HideIf("m_oneTimeOnly"), TabGroup("Exit")]
        private UnityEvent m_exitEvents;

        private bool m_wasTriggered;

        public ISaveData Save()
        {
            return new SaveData(m_wasTriggered);
        }

        public void Load(ISaveData data)
        {
            m_wasTriggered = ((SaveData)data).isTriggered;
        }
        public void Initialize()
        {
            m_wasTriggered = false;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                if ((m_oneTimeOnly && !m_wasTriggered) || !m_oneTimeOnly)
                {
                    Debug.Log("i was wrong");
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

        private void OnValidate()
        {
            DChildUtility.ValidateSensor(gameObject);
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