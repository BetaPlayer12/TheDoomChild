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
        private Dictionary<SerializeID, ISaveData> m_saveDatas = new Dictionary<SerializeID, ISaveData>(new SerializeID.EqualityComparer());

        public SerializeDataList()
        {
            m_saveDatas = new Dictionary<SerializeID, ISaveData>(new SerializeID.EqualityComparer());
        }

        public void UpdateData(SerializeID ID, ISaveData data)
        {
            if (m_saveDatas.ContainsKey(ID))
            {
                m_saveDatas[ID] = data;
            }
            else
            {
                m_saveDatas.Add(new SerializeID(ID, false), data);
            }
        }

        public ISaveData GetData(SerializeID ID)
        {
            var IDvalue = ID.value;
            return m_saveDatas.ContainsKey(ID) ? m_saveDatas[ID] : null;
        }

        public Dictionary<SerializeID, ISaveData> saveDatas => m_saveDatas;

        public SerializeDataList(SerializeDataList data)
        {
            m_saveDatas = new Dictionary<SerializeID, ISaveData>(new SerializeID.EqualityComparer());
            if (data != null)
            {
                foreach (var key in data.saveDatas.Keys)
                {
                    m_saveDatas.Add(key, data.saveDatas[key]);
                }
            }
        }
    }
}