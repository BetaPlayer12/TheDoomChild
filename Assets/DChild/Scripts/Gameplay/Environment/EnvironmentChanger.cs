using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class EnvironmentChanger : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        private struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_hasChanged;

            public SaveData(bool hasChanged)
            {
                m_hasChanged = hasChanged;
            }

            public bool hasChange => m_hasChanged;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_hasChanged);
        }

        [SerializeField, OnValueChanged("OnHasChanged")]
        private bool m_hasChanged;
        [SerializeField, TabGroup("Default")]
        private UnityEvent m_default;
        [SerializeField, TabGroup("Changed")]
        private UnityEvent m_changed;
        [SerializeField, TabGroup("Transistion")]
        private UnityEvent m_transistionToChanged;

        public void Load(ISaveData data)
        {
            m_hasChanged = ((SaveData)data).hasChange;
            if (m_hasChanged)
            {
                m_changed?.Invoke();
            }
            else
            {
                m_default?.Invoke();
            }
        }

        public ISaveData Save() => new SaveData(m_hasChanged);

        public void SetChange(bool value)
        {
            if (m_hasChanged != value)
            {
                m_hasChanged = value;
                if (m_hasChanged)
                {
                    m_transistionToChanged?.Invoke();
                }
                else
                {
                    m_default?.Invoke();
                }
            }
        }

        private void Awake()
        {
            if (m_hasChanged)
            {
                m_changed?.Invoke();
            }
            else
            {
                m_default?.Invoke();
            }
        }

#if UNITY_EDITOR
        private void OnHasChanged()
        {
            if (m_hasChanged)
            {
                m_changed?.Invoke();
            }
            else
            {
                m_default?.Invoke();
            }
        }
#endif
    }
}