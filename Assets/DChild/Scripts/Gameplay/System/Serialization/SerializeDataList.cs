using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Serialization
{
    [System.Serializable]
    public class SerializeDataList
    {
        [OdinSerialize]
        private Dictionary<int, ISaveData> m_saveDatas = new Dictionary<int, ISaveData>();

        public SerializeDataList()
        {
            m_saveDatas = new Dictionary<int, ISaveData>();
        }

        public void UpdateZoneData(SerializeDataID ID, ISaveData data)
        {
            var IDvalue = ID.value;
            if (m_saveDatas.ContainsKey(ID.value))
            {
                m_saveDatas[IDvalue] = data;
            }
            else
            {
                m_saveDatas.Add(IDvalue, data);
            }
        }

        public ISaveData GetZoneData(SerializeDataID ID)
        {
            var IDvalue = ID.value;
            return m_saveDatas.ContainsKey(IDvalue) ? m_saveDatas[IDvalue] : null;
        }
    }
}