using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay;
#if UNITY_EDITOR
using System.IO;
#endif


namespace DChild.Serialization
{
    [CreateAssetMenu(fileName = "DynamicSerializableData", menuName = "DChild/Serialization/Dynamic Serializable Data")]
    public class DynamicSerializableData : SerializedScriptableObject
    {
        public enum Type
        {
            Misc,
            Quest,
        }

        [SerializeField]
        private SerializeID m_ID = new SerializeID(true);
        [SerializeField]
        private Type m_saveType;
        [SerializeField, ReadOnly]
        private bool m_isLocked;
        [SerializeField]
        private ISaveData m_data;

        public int ID => m_ID.value;
        public bool isLocked => m_isLocked;

        public T GetData<T>() where T : ISaveData => (T)m_data;
        public void SetData(ISaveData saveData)
        {
            if (saveData != null)
            {
                m_data = saveData;
            }
        }

        public void SaveData()
        {
            if (m_saveType == Type.Misc)
            {
                GameplaySystem.campaignSerializer.slot.UpdateData(m_ID, m_data);
            }
            else
            {
                GameplaySystem.campaignSerializer.slot.UpdateCampaignProgress(m_ID, m_data);
            }
        }

        public void LoadData()
        {
            if (m_saveType == Type.Misc)
            {
                m_data = GameplaySystem.campaignSerializer.slot.GetData<ISaveData>(m_ID);
            }
            else
            {
                m_data = GameplaySystem.campaignSerializer.slot.GetCampaignProgress<ISaveData>(m_ID);
            }
        }

        public void SetLock(bool isLocked) => m_isLocked = isLocked;
    }
}