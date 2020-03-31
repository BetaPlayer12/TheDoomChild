using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay;
#if UNITY_EDITOR
using System.IO;
#endif


namespace DChild.Serialization
{
    [CreateAssetMenu(fileName = "DynamicSerializableData", menuName = "DChild/Serialization/Dynamic Serializable Data")]
    public class DynamicSerializableData : ScriptableObject
    {
        [SerializeField]
        private SerializeID m_ID = new SerializeID(true);
        [SerializeField, ReadOnly]
        private bool m_isLocked;
        [ShowInInspector]
        private ISaveData m_data;

        public int ID => m_ID.value;
        public bool isLocked => m_isLocked;

        public T GetData<T>() where T : ISaveData => (T)m_data;
        public void SetData(ISaveData saveData) => m_data = saveData;
        public void SaveData() => GameplaySystem.campaignSerializer.slot.UpdateData(m_ID, m_data);
        public void LoadData() => m_data = GameplaySystem.campaignSerializer.slot.GetData<ISaveData>(m_ID);

        public void SetLock(bool isLocked) => m_isLocked = isLocked;
    }
}