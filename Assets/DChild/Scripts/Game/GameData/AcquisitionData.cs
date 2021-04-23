using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Serialization
{
    [System.Serializable]
    public class AcquisitionData
    {
        [System.Serializable]
        public struct SerializeData
        {
            [SerializeField]
            private int m_ID;
            [SerializeField]
            private bool m_hasData;

            public SerializeData(int iD, bool hasData)
            {
                m_ID = iD;
                this.m_hasData = hasData;
            }

            public int ID => m_ID;
            public bool hasData => m_hasData;
        }

        [SerializeField, TableList(NumberOfItemsPerPage = 5, ShowPaging = true), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, NumberOfItemsPerPage = 5, ShowPaging = true)]
        private SerializeData[] m_serializeDatas;

        public int count => m_serializeDatas.Length;

        public SerializeData GetData(int index) => m_serializeDatas[index];

        public AcquisitionData(SerializeData[] m_serializeDatas)
        {
            this.m_serializeDatas = m_serializeDatas;
        }

        public AcquisitionData()
        {
            m_serializeDatas = new SerializeData[1];
        }

        public AcquisitionData(AcquisitionData data)
        {
            m_serializeDatas = new SerializeData[data.count];
            for (int i = 0; i < data.count; i++)
            {
                m_serializeDatas[i] = data.GetData(i);
            }
        }
    }
}