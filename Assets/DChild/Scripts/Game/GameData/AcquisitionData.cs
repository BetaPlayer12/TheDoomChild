using UnityEngine;

namespace DChild.Serialization
{
    [System.Serializable]
    public class AcquisitionData
    {
        [System.Serializable]
        public struct SerializeData
        {
            public int ID { get; }
            public bool hasData { get; }

            public SerializeData(int iD, bool hasData)
            {
                ID = iD;
                this.hasData = hasData;
            }
        }

        [SerializeField]
        private SerializeData[] m_serializeDatas;

        public int count => m_serializeDatas.Length;

        public SerializeData GetData(int index) => m_serializeDatas[index];

        public AcquisitionData(SerializeData[] m_serializeDatas)
        {
            this.m_serializeDatas = m_serializeDatas;
        }
    }
}