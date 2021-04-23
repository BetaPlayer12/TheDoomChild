using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay
{
    public class EventTrigger : MonoBehaviour, ISerializableComponent
    {
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_isTriggered;

            public SaveData(bool isTriggered)
            {
                m_isTriggered = isTriggered;
            }

            public bool isTriggered => m_isTriggered;

            public ISaveData ProduceCopy()
            {
                return new SaveData(m_isTriggered);
            }
        }

        private bool m_isTriggered;

        public void Load(ISaveData data)
        {
            m_isTriggered = ((SaveData)data).isTriggered;
            if (m_isTriggered)
            {
                m_postTrigger?.Invoke();
            }
            else
            {
                m_preTrigger?.Invoke();
            }
        }

        public ISaveData Save()
        {
            return new SaveData(m_isTriggered);
        }

        [SerializeField, TabGroup("Pre")]
        private UnityEvent m_preTrigger;
        [SerializeField, TabGroup("During")]
        private UnityEvent m_duringTrigger;
        [SerializeField, TabGroup("post")]
        private UnityEvent m_postTrigger;

        public void Execute()
        {
            m_duringTrigger?.Invoke();
            m_isTriggered = true;
        }
    }
}