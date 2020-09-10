using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay
{
    public class MultiSceneUnlockableEvent : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public class Requirement
        {
            [SerializeField, AssetsOnly]
            private DynamicSerializableData m_requirement;

            [FoldoutGroup("Reactions")]
            [SerializeField, HorizontalGroup("Reactions/Split")]
            private UnityEvent m_onComplete;
            [SerializeField, HorizontalGroup("Reactions/Split")]
            private UnityEvent m_onIncomplete;

            public bool isComplete => m_requirement.GetData<IUnlockableRequirementSaveData>().isComplete;

            public void UpdateRequirement()
            {
                m_requirement.LoadData();
                if (m_requirement.GetData<IUnlockableRequirementSaveData>().isComplete)
                {
                    m_onComplete?.Invoke();
                }
                else
                {
                    m_onIncomplete?.Invoke();
                }
            }
        }

        [System.Serializable]
        public struct SaveData : ISaveData
        {
            public SaveData(bool isUnlocked) : this()
            {
                this.m_isUnlocked = isUnlocked;

            }

            [SerializeField]
            private bool m_isUnlocked;

            public bool isUnlocked => m_isUnlocked;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isUnlocked);
        }

        [ShowInInspector, OnValueChanged("OnStateChange")]
        private bool m_isUnlocked;
        [SerializeField, TabGroup("Requirements")]
        private Requirement[] m_requirements;
        [SerializeField, TabGroup("Unlocked")]
        private UnityEvent m_alreadyUnlocked;
        [SerializeField, TabGroup("Locked")]
        private UnityEvent m_onLocked;

        public void Load(ISaveData data)
        {
            SaveData saveData = (SaveData)data;
            m_isUnlocked = saveData.isUnlocked;
            if (m_isUnlocked)
            {
                m_alreadyUnlocked?.Invoke();
            }
            else
            {
                m_onLocked?.Invoke();
                bool isComplete = true;
                for (int i = 0; i < m_requirements.Length; i++)
                {
                    m_requirements[i].UpdateRequirement();
                    if (isComplete)
                    {
                        isComplete = m_requirements[i].isComplete;
                    }
                }
            }

            //Find a way to do both multiscene and same scene;
        }

        public ISaveData Save()
        {
            return new SaveData(m_isUnlocked);
        }

#if UNITY_EDITOR
        private void OnStateChange()
        {
            if (m_isUnlocked)
            {
                m_alreadyUnlocked?.Invoke();
            }
            else
            {
                m_onLocked?.Invoke();
            }
        }
#endif
    }
}